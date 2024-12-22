using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

public class TransparencyController : MonoBehaviour
{
    [SerializeField, Required] private float _targetTransparency = 0.5f;
    [SerializeField, Required] private float _fadeDuration = 0.5f;
    private readonly List<Material> _materials = new();
    private readonly List<Tweener> _tweeners = new();

    private void Awake()
    {
        var meshRenderers = GetComponentsInChildren<MeshRenderer>();
        foreach (var mr in meshRenderers)
        {
            if (mr.materials.Length == 0) continue;

            _materials.Add(mr.materials[0]);
            _tweeners.Add(mr.materials[0].DOFade(1.0f, 0.0f));
        }
    }

    private void OnEnable()
    {
        foreach (var tw in _tweeners)
        {
            tw?.Kill();
        }
    }

    public void FadeIn()
    {
        for (var i = 0; i < _materials.Count; i++)
        {
            _tweeners[i]?.Kill();
            _tweeners[i] = _materials[i].DOFade(_targetTransparency, _fadeDuration);
        }
    }

    public void FadeOut()
    {
        for (var i = 0; i < _materials.Count; i++)
        {
            _tweeners[i]?.Kill();
            _tweeners[i] = _materials[i].DOFade(1.0f, _fadeDuration);
        }
    }
}