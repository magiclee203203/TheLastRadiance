using System;
using DG.Tweening;
using Febucci.UI;
using Febucci.UI.Core.Parsing;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OutroPanel : MonoBehaviour, IPanel
{
    [SerializeField, Required, SceneObjectsOnly]
    private Image _backgroundImage;

    [SerializeField, Required, SceneObjectsOnly]
    private TMP_Text _contentText;

    [SerializeField, Required, SceneObjectsOnly]
    private TMP_Text _continueText;

    [SerializeField, Required, AssetsOnly] private SOOutroConfig _config;
    [SerializeField, Required] private float _fadeDuration = 1.0f;
    [SerializeField, Required] private float _continueFlashDuration = 0.8f;

    [SerializeField, Required, SceneObjectsOnly]
    private TypewriterByCharacter _typewriter;

    [HideInInspector] public Action OnEnd;
    private bool _enableContinue;

    private void Awake()
    {
        _continueText.alpha = 0.0f;

        var bgColor = _backgroundImage.color;
        bgColor.a = 0.0f;
        _backgroundImage.color = bgColor;
    }

    private void OnEnable()
    {
        _typewriter.onMessage.AddListener(TypeWriterEvent);
        BgImageFadeIn(() => { _typewriter.ShowText(_config.Content); });
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
        OnEnd?.Invoke();
    }

    private void BgImageFadeIn(Action onComplete)
    {
        _backgroundImage.DOFade(1.0f, _fadeDuration).OnComplete(() => { onComplete?.Invoke(); });
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
        _continueText.DOFade(0.8f, _continueFlashDuration)
            .SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InQuart);
        _enableContinue = true;
    }
}