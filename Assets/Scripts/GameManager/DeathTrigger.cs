using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class DeathTrigger : MonoBehaviour
{
    [SerializeField, Required, AssetsOnly] [BoxGroup("Events Published"), LabelText("Player Death")]
    private SOPlayerDeathEvent _playerDeathEvent;

    [SerializeField, Required] private string _roomName;
    [SerializeField, Required] private float _waitTimeToTriggerFadeOut;

    private BoxCollider _collider;

    public string RoomName => _roomName;

    private void Awake()
    {
        _collider = GetComponent<BoxCollider>();
        _collider.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(VariableNamesDefine.PlayerTag)) return;
        StartCoroutine(WaitForTriggerFadeOut(_waitTimeToTriggerFadeOut));
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag(VariableNamesDefine.CrateTag)) return;

        if (GlobalVariablesManager.Instance.GetValue(VariableNamesDefine.TargetGravityObject,
                out GameObject targetGravityObject) && targetGravityObject == gameObject)
            GlobalVariablesManager.Instance.RemoveValue(VariableNamesDefine.TargetGravityObject);

        Destroy(other.gameObject);
    }

    private IEnumerator WaitForTriggerFadeOut(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        _playerDeathEvent.Notify(this);
    }
}