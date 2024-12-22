using System;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

public class HiddenBridge : SerializedMonoBehaviour
{
    [SerializeField, Required, LabelText("Init Show State")]
    private bool _initShow;

    [SerializeField, Required] private int _lockNum;

    [SerializeField, AssetsOnly] [BoxGroup("Events Subscribed"), LabelText("Activate")]
    private SOOperateLockEvent _activateEvent;

    [SerializeField, Required] private float _transitionDuration = 1.0f;
    [SerializeField] private List<ICommand> _tasksAfterFadeIn = new();
    [SerializeField] private List<ICommand> _tasksAfterFadeOut = new();

    [SerializeField] private bool _currentShowState;
    private MeshRenderer _meshRenderer;
    private Material _mat;
    private Collider _collider;
    private Tweener _tweener;

    private void Awake()
    {
        _currentShowState = _initShow;
        _meshRenderer = GetComponent<MeshRenderer>();
        _mat = _meshRenderer.materials[0];
        _collider = GetComponent<Collider>();

        if (!_initShow)
            Hide(() => { });
    }

    private void OnEnable()
    {
        _activateEvent.Subscribe(Activate);
    }

    private void OnDisable()
    {
        _activateEvent.Unsubscribe(Activate);
        _tweener?.Kill();
    }

    private void Activate(int activeLockNum)
    {
        if (_lockNum != activeLockNum) return;
        _currentShowState = !_currentShowState;
        HandleState(_currentShowState);
    }

    private void HandleState(bool newShowState)
    {
        if (newShowState)
            FadeIn();
        else
            FadeOut();
    }

    private void FadeIn()
    {
        _tweener?.Kill(true);

        // Must Be Here! Not in OnComplete!
        _meshRenderer.enabled = true;
        _tweener = _mat.DOFade(1.0f, _transitionDuration).OnComplete(() =>
        {
            _collider.enabled = true;
            foreach (var task in _tasksAfterFadeIn)
            {
                task.Execute(this);
            }
        });

        AudioManager.Instance.Play(AudioManager.SoundType.HiddenBridgeShow);
    }

    private void FadeOut()
    {
        _tweener?.Kill(true);
        _tweener = _mat.DOFade(0.0f, _transitionDuration).OnComplete(() =>
        {
            _collider.enabled = false;
            foreach (var task in _tasksAfterFadeOut)
            {
                task.Execute(this);
            }
        }).OnUpdate(() =>
        {
            if (_mat.color.a < 0.01f)
                _meshRenderer.enabled = false;
        });
    }

    private void Hide(Action onComplete)
    {
        _currentShowState = false;

        _tweener?.Kill(true);
        _tweener = _mat.DOFade(0.0f, 0.0f).OnComplete(() =>
        {
            _meshRenderer.enabled = false;
            _collider.enabled = false;
            onComplete.Invoke();
        });
    }
}