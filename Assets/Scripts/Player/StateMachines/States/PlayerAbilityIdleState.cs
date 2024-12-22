using Animancer.FSM;
using Sirenix.OdinInspector;
using UnityEngine;

public class PlayerAbilityIdleState : PlayerBaseState
{
    [SerializeField, Required, AssetsOnly] [BoxGroup("Events published"), LabelText("Cancel Interactive Sphere")]
    private SOEvent _cancelInteractiveSphereEvent;

    private void OnEnable()
    {
        // come from charge state
        if (this.GetPreviousState<PlayerBaseState>() is PlayerAbilityChargeState)
        {
            _cancelInteractiveSphereEvent.Notify();
        }
    }
}