using System;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class PausePanel : MonoBehaviour, IPanel
{
    public enum ButtonType
    {
        Continue,
        QuitGame
    }

    [SerializeField, Required, SceneObjectsOnly]
    List<Button> _menuButtons = new();

    [SerializeField, Required] private float _transitionDuration = 0.5f;
    [SerializeField, Required, AssetsOnly] private Sprite _selectedSprite;
    [SerializeField, Required, AssetsOnly] private Sprite _unselectedSprite;

    [HideInInspector] public Action<ButtonType> OnConfirm;

    private RectTransform _rectTransform;
    private int _selectedIndex;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
    }

    private void OnEnable()
    {
        _rectTransform.localScale = Vector3.zero;
        _rectTransform.DOScale(Vector3.one, _transitionDuration).SetEase(Ease.OutBounce).SetUpdate(true);

        ChangeAppearance();
    }

    private void OnDisable()
    {
        _selectedIndex = 0;
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        _rectTransform.DOScale(Vector3.zero, _transitionDuration).SetEase(Ease.InOutQuint).OnComplete(() =>
        {
            gameObject.SetActive(false);
        });
    }

    public void Continue()
    {
        switch (_selectedIndex)
        {
            case 0:
                OnConfirm?.Invoke(ButtonType.Continue);
                break;
            case 1:
                OnConfirm?.Invoke(ButtonType.QuitGame);
                break;
        }

        AudioManager.Instance.Play(AudioManager.SoundType.ConfirmInPauseMenu);
    }

    public void SelectButton(int index)
    {
        _selectedIndex += index;
        if (_selectedIndex >= _menuButtons.Count)
            _selectedIndex = _menuButtons.Count - 1;

        if (_selectedIndex < 0)
            _selectedIndex = 0;

        ChangeAppearance();
        AudioManager.Instance.Play(AudioManager.SoundType.MovePauseMenuButton);
    }

    private void ChangeAppearance()
    {
        for (var i = 0; i < _menuButtons.Count; i++)
        {
            _menuButtons[i].image.sprite = i == _selectedIndex ? _selectedSprite : _unselectedSprite;
        }
    }
}