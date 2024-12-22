using Sirenix.OdinInspector;
using UnityEngine;

public class NotifyDialogueEndedCommand : ICommand
{
    [SerializeField, Required] private string _endedConversationTitle;

    [SerializeField, Required, AssetsOnly] [BoxGroup("Events Published"), LabelText("Dialogue Ended")]
    private SODialogueTriggerEvent _notifyEvent;

    public void Execute<T>(T receiver)
    {
        _notifyEvent.Notify(_endedConversationTitle);
    }
}