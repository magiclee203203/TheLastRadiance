using System;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StartPanel : SerializedMonoBehaviour, IPanel
{
    [SerializeField, Required, SceneObjectsOnly]
    private CanvasGroup _foregroundGroup;

    [SerializeField, Required, SceneObjectsOnly]
    private Image _maskImage;

    [SerializeField, Required, SceneObjectsOnly]
    private TMP_Text _startText;

    [SerializeField, Required] private float _maskTransitionDuration = 0.5f;
    [SerializeField, Required] private float _startTextTransitionDuration = 0.5f;
    [SerializeField, Required] private List<ICommand> _tasksAfterQuit = new();
    [HideInInspector] public Action<List<ICommand>> OnEnd;

    private void Awake()
    {
        var color = _startText.color;
        color.a = 0.0f;
        _startText.color = color;
    }

    public void Show()
    {
        _maskImage.rectTransform.DOMoveX(3000.0f, _maskTransitionDuration)
            .SetRelative().SetEase(Ease.InQuart).OnComplete(ShowStartText);
    }

    public void Continue()
    {
        _foregroundGroup.DOFade(0.0f, _maskTransitionDuration)
            .OnComplete(() =>
            {
                Destroy(gameObject);
                OnEnd?.Invoke(_tasksAfterQuit);
            });
    }

    private void ShowStartText()
    {
        _startText.DOFade(0.8f, _startTextTransitionDuration)
            .SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutQuad);
    }
}