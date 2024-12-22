using System.Collections;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

public class Floating : MonoBehaviour
{
    [SerializeField, Required] private float _floatingDistance = 0.5f;
    [SerializeField, Required] private float _floatingDuration = 1.0f;

    private Tweener _tweener;

    private void Start()
    {
        StartCoroutine(WaitForRandomSeconds());
    }

    private void OnDisable()
    {
        _tweener?.Kill();
    }

    private IEnumerator WaitForRandomSeconds()
    {
        var startTime = Random.Range(0.0f, _floatingDuration * 1.5f);
        yield return new WaitForSeconds(startTime);
        StartFloating();
    }

    private void StartFloating()
    {
        _tweener = transform.DOMoveY(_floatingDistance, _floatingDuration)
            .SetRelative().SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);
    }
}