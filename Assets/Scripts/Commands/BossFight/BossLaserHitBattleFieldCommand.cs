using Sirenix.OdinInspector;
using UnityEngine;

public class BossLaserHitBattleFieldCommand : ICommand
{
    [SerializeField, Required, AssetsOnly] [BoxGroup("Events Published"), LabelText("Hit Battle Field")]
    private SOEvent _hitBattleFieldEvent;

    public void Execute<T>(T receiver)
    {
        if (receiver is not Collision other) return;
        if (other.gameObject.CompareTag(VariableNamesDefine.PlayerTag)) return;
        if (other.gameObject.TryGetComponent(out EnergyLaserTurret _)) return;

        _hitBattleFieldEvent.Notify();
    }
}