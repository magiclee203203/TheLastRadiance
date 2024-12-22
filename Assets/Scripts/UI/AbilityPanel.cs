using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class AbilityPanel : SerializedMonoBehaviour, IPanel
{
    [SerializeField, Required, SceneObjectsOnly]
    private Image _indicator;

    [SerializeField, Required] private Dictionary<string, Sprite> _sprites;

    [SerializeField, Required] private float _fadeDuration = 0.5f;
    [SerializeField, Required] private float _popupDuration = 0.5f;

    private string _activeAbilityName = "";

    private Tweener _fadeTweener;
    private Tweener _popupTweener;

    private void Awake()
    {
        HideIndicator();
    }

    public void Show()
    {
        HideIndicator();
        Popup();
    }

    public void Continue()
    {
    }

    private void HideIndicator()
    {
        var color = _indicator.color;
        color.a = 0.0f;
        _indicator.color = color;

        _indicator.rectTransform.localScale = Vector3.zero;
    }

    private void Popup()
    {
        _fadeTweener?.Kill();
        _popupTweener?.Kill();

        _fadeTweener = _indicator.DOFade(1.0f, _fadeDuration);
        _popupTweener = _indicator.rectTransform.DOScale(1.0f, _popupDuration).SetEase(Ease.OutBack);
    }

    public void ShowAbility(string abilityName)
    {
        if (_sprites.TryGetValue(abilityName, out var sprite))
            _indicator.sprite = sprite;

        HideIndicator();
        Popup();

        _activeAbilityName = abilityName;
    }

    public void HideAbility()
    {
        HideIndicator();
    }

    public void ReShow()
    {
        if (_activeAbilityName == "") return;
        ShowAbility(_activeAbilityName);
    }
}