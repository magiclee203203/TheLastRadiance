using Animancer;
using Sirenix.OdinInspector;
using UnityEngine;

public class AutoGate : MonoBehaviour
{
    [SerializeField, Required, SceneObjectsOnly]
    private GateTrigger _openTrigger;

    [SerializeField, SceneObjectsOnly] [InfoBox("Leave it to None if you don't want it to close when pass through")]
    private GateTrigger _closeTrigger;

    [SerializeField, Required, AssetsOnly] private TransitionAsset _openAnimation;

    private AnimancerComponent _animancer;
    private AnimancerState _animationState;

    private bool _hasOpened;
    private bool _hasClosed;

    private void Awake()
    {
        _animancer = GetComponent<AnimancerComponent>();
    }

    private void OnEnable()
    {
        _openTrigger.Callback += OnOpen;
        if (_closeTrigger != null) _closeTrigger.Callback += OnClose;
    }

    private void OnDisable()
    {
        _openTrigger.Callback -= OnOpen;
        if (_closeTrigger != null) _closeTrigger.Callback -= OnClose;
    }

    private void OnOpen()
    {
        _animationState = _animancer.Play(_openAnimation);
        _animationState.Speed = _openAnimation.Speed;
        _animationState.IsPlaying = true;
        _animationState.Events(this).OnEnd ??= () => { _animationState.IsPlaying = false; };

        if (_hasOpened) return;
        AudioManager.Instance.Play(AudioManager.SoundType.DoorOperation);
        _hasOpened = true;
    }

    private void OnClose()
    {
        _animationState = _animancer.Play(_openAnimation);
        _animationState.Speed = _openAnimation.Speed * -1.0f;
        _animationState.IsPlaying = true;

        if (_hasClosed) return;
        AudioManager.Instance.Play(AudioManager.SoundType.DoorOperation);
        _hasClosed = true;
    }
}