using Sirenix.OdinInspector;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class MyDialogueTrigger : SerializedMonoBehaviour
{
    enum TriggerMode
    {
        OnTriggerEnter,
        OnTriggerExit,
    }

    [SerializeField, Required, EnumToggleButtons]
    private TriggerMode _triggerMode;

    [SerializeField, Required] private string _conversationTitle;

    [SerializeField, Required, AssetsOnly] [BoxGroup("Events Published"), LabelText("Trigger Dialogue")]
    private SODialogueTriggerEvent _dialogueTriggerEvent;

    [SerializeField] private ICondition _condition;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(VariableNamesDefine.PlayerTag) || _triggerMode != TriggerMode.OnTriggerEnter) return;

        if (_condition == null || _condition.IsConditionMet())
            _dialogueTriggerEvent.Notify(_conversationTitle);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag(VariableNamesDefine.PlayerTag) || _triggerMode != TriggerMode.OnTriggerExit) return;

        if (_condition == null || _condition.IsConditionMet())
            _dialogueTriggerEvent.Notify(_conversationTitle);
    }
}