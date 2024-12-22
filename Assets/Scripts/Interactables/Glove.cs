using System;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

public class Glove : SerializedMonoBehaviour
{
    [SerializeField, Required, SceneObjectsOnly]
    private GameObject _gloveModel;

    [SerializeField, Required] private float _rotateDuration;
    [SerializeField, Required] private ICommand _taskAfterTriggered;

    private Tweener _tweener;

    private void Awake()
    {
        var currentRot = _gloveModel.transform.localEulerAngles;
        var targetRot = currentRot + new Vector3(0f, 360.0f, 0f);

        _tweener = _gloveModel.transform.DOLocalRotate(targetRot, _rotateDuration, RotateMode.FastBeyond360)
            .SetEase(Ease.Linear).SetLoops(-1, LoopType.Restart);
    }

    private void OnDisable()
    {
        _tweener?.Kill();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(VariableNamesDefine.PlayerTag)) return;
        _taskAfterTriggered.Execute(this);
        Destroy(gameObject);
    }
}