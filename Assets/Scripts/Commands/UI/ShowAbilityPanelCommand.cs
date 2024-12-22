using Sirenix.OdinInspector;
using UnityEngine;

public class ShowAbilityPanelCommand : ICommand
{
    [SerializeField, Required, AssetsOnly] [BoxGroup("Events Published"), LabelText("Show Ability")]
    private SOShowAbilityIndicatorEvent _showAbilityEvent;

    [SerializeField, Required, AssetsOnly] private SOAbilityConfig _abilityCfg;

    public void Execute<T>(T receiver)
    {
        _showAbilityEvent.Notify(_abilityCfg.Name);
    }
}