using System;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopupPanel : MonoBehaviour, IPanel
{
    [SerializeField, Required] private float _transitionDuration = 0.5f;

    [SerializeField, Required, SceneObjectsOnly]
    private TMP_Text _title;

    [SerializeField, Required, SceneObjectsOnly]
    private Image _img;

    [SerializeField, Required, SceneObjectsOnly]
    private TMP_Text _tutorial;

    [HideInInspector] public Action<List<ICommand>> OnClose;

    private RectTransform _rectTransform;
    private SOPopupPanelConfig _config;

    public SOPopupPanelConfig Config
    {
        set => _config = value;
    }

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
    }

    private void OnEnable()
    {
        _rectTransform.localScale = Vector3.zero;
        _rectTransform.DOScale(Vector3.one, _transitionDuration).SetEase(Ease.OutBounce);

        // update UI
        _title.text = _config.Title;
        _img.rectTransform.sizeDelta = new Vector2(_config.ImageWidth, _config.ImageHeight);
        _img.sprite = _config.Image;
        if (_config.Tutorial != null)
            _tutorial.text = _config.Tutorial;
        else
            _tutorial.enabled = false;
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Continue()
    {
        _rectTransform.DOScale(Vector3.zero, _transitionDuration).SetEase(Ease.InOutQuint).OnComplete(() =>
        {
            gameObject.SetActive(false);
            OnClose?.Invoke(_config.TasksAfterClose);
        });
    }
}