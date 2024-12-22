using Sirenix.OdinInspector;
using UnityEngine;

public class ShowOutroPanelCommand : ICommand
{
    [SerializeField, Required, AssetsOnly] [BoxGroup("Events Published"), LabelText("Show Outro Panel")]
    private SOEvent _showOutroPanelEvent;

    public void Execute<T>(T receiver)
    {
        _showOutroPanelEvent.Notify();
    }
}