using System;
using System.Collections;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDeathPanel : MonoBehaviour, IPanel
{
    [SerializeField, Required] private float _transitionDuration = 0.25f;
    [SerializeField, Required] private float _delayBetweenTransition = 0.25f;

    [HideInInspector] public Action OnEnd;

    private Image _image;
    private Coroutine _transitionCoroutine;
    private Tweener _tweener;

    private void Awake()
    {
        _image = GetComponent<Image>();
    }

    private void OnEnable()
    {
        if (_transitionCoroutine != null)
            StopCoroutine(_transitionCoroutine);

        _transitionCoroutine = StartCoroutine(StartFadeInAndOut());
    }

    private IEnumerator StartFadeInAndOut()
    {
        _tweener?.Kill();

        // fade in firstly
        _tweener = _image.DOFade(1.0f, _transitionDuration);
        yield return _tweener.WaitForCompletion();

        // wait
        yield return new WaitForSeconds(_delayBetweenTransition);

        // fade out
        _tweener = _image.DOFade(0.0f, _transitionDuration);
        yield return _tweener.WaitForCompletion();

        _transitionCoroutine = null;
        gameObject.SetActive(false);

        OnEnd?.Invoke();
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Continue()
    {
    }
}