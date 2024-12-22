using Sirenix.OdinInspector;
using UnityEngine;

public class TriggerDialogueCommand : ICommand
{
    [SerializeField, Required, AssetsOnly] [BoxGroup("Events Published"), LabelText("Trigger Dialogue")]
    private SODialogueTriggerEvent _dialogueTriggerEvent;

    [SerializeField, Required] private string _triggeredConversationTitle;

    public void Execute<T>(T receiver)
    {
        _dialogueTriggerEvent.Notify(_triggeredConversationTitle);
    }
}