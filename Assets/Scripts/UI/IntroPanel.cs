using System;
using System.Collections.Generic;
using DG.Tweening;
using Febucci.UI;
using Febucci.UI.Core.Parsing;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IntroPanel : SerializedMonoBehaviour, IPanel
{
    [SerializeField, Required, SceneObjectsOnly]
    private CanvasGroup _foregroundGroup;

    [SerializeField, Required, SceneObjectsOnly]
    private Image _backgroundGroup;

    [SerializeField, Required, SceneObjectsOnly]
    private Image _backgroundImage;

    [SerializeField, Required, SceneObjectsOnly]
    private TMP_Text _introText;

    [SerializeField, Required, SceneObjectsOnly]
    private TMP_Text _continueText;

    [SerializeField, Required, SceneObjectsOnly]
    private TypewriterByCharacter _typewriter;

    [SerializeField, Required] private float _fadeDuration;
    [SerializeField, Required] private float _continueFlashDuration;
    [SerializeField, Required, AssetsOnly] private List<SOIntroConfig> _introConfigs;
    [SerializeField, Required] private List<ICommand> _tasksAfterQuit = new();
    [HideInInspector] public Action<List<ICommand>> OnEnd;

    private int _currentIntroIndex = 0;
    private bool _enableContinue;
    private Tweener _continueFlashTweener;

    private void Awake()
    {
        _foregroundGroup.alpha = 0.0f;
        _continueText.alpha = 0.0f;
    }

    private void OnEnable()
    {
        _typewriter.onMessage.AddListener(TypeWriterEvent);
        ContentFadeIn();
    }

    private void OnDisable()
    {
        _typewriter.onMessage.RemoveListener(TypeWriterEvent);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Continue()
    {
        if (!_enableContinue) return;

        if (_currentIntroIndex < _introConfigs.Count - 1)
        {
            _continueFlashTweener?.Kill();
            _enableContinue = false;
            _continueText.alpha = 0.0f;

            ContentFadeOut(() =>
            {
                _introText.text = "";
                _currentIntroIndex++;
                ContentFadeIn();
            });
        }
        else
        {
            _foregroundGroup.DOFade(0.0f, _fadeDuration)
                .OnComplete(() =>
                {
                    Destroy(gameObject);
                    OnEnd?.Invoke(_tasksAfterQuit);
                });
            _backgroundGroup.DOFade(0.0f, _fadeDuration);
        }
    }

    private void TypeWriterEvent(EventMarker eventMarker)
    {
        switch (eventMarker.name)
        {
            case VariableNamesDefine.IntroContentEndAppearing:
                ShowContinueTip();
                break;
        }
    }

    private void ShowContinueTip()
    {
        _continueFlashTweener = _continueText.DOFade(0.8f, _continueFlashDuration)
            .SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InQuart);
        _enableContinue = true;
    }

    private void ContentFadeIn()
    {
        var cfg = _introConfigs[_currentIntroIndex];
        _backgroundImage.sprite = cfg.Background;
        _foregroundGroup.DOFade(1.0f, _fadeDuration).OnComplete(() => { _typewriter.ShowText(cfg.Content); });
    }

    private void ContentFadeOut(Action onComplete)
    {
        _foregroundGroup.DOFade(0.0f, _fadeDuration).OnComplete(() => { onComplete?.Invoke(); });
    }
}