using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;

public class ResetAppearanceCommand : ICommand
{
    [SerializeField, Required] private float _resetInterval = 2.0f;
    private Coroutine _resetCoroutine;

    [SerializeField, Required, AssetsOnly] private Material _defaultMaterial;

    private EnergyBallSwitch _energyBallSwitch;

    public void Execute<T>(T receiver)
    {
        if (receiver is not EnergyBallSwitch ebs) return;
        _energyBallSwitch = ebs;

        if (_resetCoroutine != null)
            _energyBallSwitch.StopCoroutine(_resetCoroutine);

        _resetCoroutine = _energyBallSwitch.StartCoroutine(WaitForReset(_resetInterval));
    }

    private IEnumerator WaitForReset(float interval)
    {
        yield return new WaitForSeconds(interval);
        _energyBallSwitch.CurrentMaterial = _defaultMaterial;
    }
}