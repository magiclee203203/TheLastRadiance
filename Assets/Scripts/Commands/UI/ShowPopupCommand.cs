using Sirenix.OdinInspector;
using UnityEngine;

public class ShowPopupCommand : ICommand
{
    [SerializeField, Required, AssetsOnly] [BoxGroup("Events Published"), LabelText("Show Popup")]
    private SOShowPopupEvent _showPopupEvent;

    [SerializeField, Required, AssetsOnly] private SOPopupPanelConfig _config;

    public void Execute<T>(T receiver)
    {
        _showPopupEvent.Notify(_config);
    }
}