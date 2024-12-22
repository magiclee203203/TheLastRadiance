using Sirenix.OdinInspector;
using UnityEngine;

public class ChangeAbilityCommand : ICommand
{
    [SerializeField, Required, AssetsOnly] [BoxGroup("Events Published"), LabelText("Change Active Ability")]
    private SOEvent _changeActiveAbilityEvent;

    public void Execute<T>(T receiver)
    {
        _changeActiveAbilityEvent.Notify();
    }
}