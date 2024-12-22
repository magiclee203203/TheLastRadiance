using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;

public class FightTurretActivateCommand : ICommand
{
    [SerializeField, Required, AssetsOnly] [BoxGroup("Events Published"), LabelText("Fight Turret Activate")]
    private SOEvent _fightTurretActivateEvent;

    [SerializeField, Required] private float _waitForNotify;

    public void Execute<T>(T receiver)
    {
        if (receiver is not EnergyLaserTurret turret) return;
        turret.StartCoroutine(Activate());
    }

    private IEnumerator Activate()
    {
        yield return new WaitForSeconds(_waitForNotify);
        _fightTurretActivateEvent.Notify();
    }
}