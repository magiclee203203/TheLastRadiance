using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class InteractiveSphereDetection : MonoBehaviour
{
    [SerializeField, Required, EnumToggleButtons]
    private InteractableType _interactableItemType;

    [SerializeField, Required, AssetsOnly] [BoxGroup("Events Subscribed"), LabelText("Disappear")]
    private SOEvent _disappearEvent;

    private readonly List<IInteractable> _cachedInteractables = new();
    private bool _isInteractiveSphereDisappearing;

    private void OnEnable()
    {
        _disappearEvent.Subscribe(OnInteractiveSphereDisappear);

        // save to global variables in order to remove when player is dead
        GlobalVariablesManager.Instance.SetValue(VariableNamesDefine.AbilitySphere, gameObject);
    }

    private void OnDisable()
    {
        _disappearEvent.Unsubscribe(OnInteractiveSphereDisappear);
        GlobalVariablesManager.Instance.RemoveValue(VariableNamesDefine.AbilitySphere);
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (_interactableItemType)
        {
            case InteractableType.EnergyBallReceiver:
                if (!other.TryGetComponent(out EnergyBallReceiver energyBallReceiver) ||
                    !energyBallReceiver.enabled) break;
                energyBallReceiver.IsDetected();
                SetTarget(VariableNamesDefine.TargetEnergyBallReceiver, energyBallReceiver);
                _cachedInteractables.Add(energyBallReceiver);

                break;

            case InteractableType.GravityObject:
                if (!other.TryGetComponent(out GravityObject gravityObject) || !gravityObject.enabled) break;
                gravityObject.IsDetected();
                SetTarget(VariableNamesDefine.TargetGravityObject, gravityObject);
                _cachedInteractables.Add(gravityObject);

                break;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        IInteractable newTarget = null;

        foreach (var interactable in _cachedInteractables)
        {
            if (interactable.Obj != other.gameObject) continue;

            newTarget = interactable;
            break;
        }

        switch (_interactableItemType)
        {
            case InteractableType.EnergyBallReceiver when newTarget != null:
                SetTarget(VariableNamesDefine.TargetEnergyBallReceiver, newTarget);
                break;

            case InteractableType.GravityObject when newTarget != null:
                SetTarget(VariableNamesDefine.TargetGravityObject, newTarget);
                break;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        switch (_interactableItemType)
        {
            case InteractableType.EnergyBallReceiver:
                if (!other.TryGetComponent(out EnergyBallReceiver energyBallReceiver)) break;
                energyBallReceiver.IsUndetected();
                UnsetTarget(VariableNamesDefine.TargetEnergyBallReceiver, energyBallReceiver);
                _cachedInteractables.Remove(energyBallReceiver);

                break;

            case InteractableType.GravityObject:
                if (!other.TryGetComponent(out GravityObject gravityObject)) break;
                gravityObject.IsUndetected();
                UnsetTarget(VariableNamesDefine.TargetGravityObject, gravityObject);
                _cachedInteractables.Remove(gravityObject);

                break;
        }
    }

    private void SetTarget(string key, IInteractable target)
    {
        if (GlobalVariablesManager.Instance.HasKey(key)) return;
        if (_isInteractiveSphereDisappearing) return;

        GlobalVariablesManager.Instance.SetValue(key, target.Obj);
        target.IsSetTarget();
    }

    private void UnsetTarget(string key, IInteractable target)
    {
        if (!GlobalVariablesManager.Instance.GetValue(key, out GameObject storedTarget) ||
            storedTarget != target.Obj) return;

        GlobalVariablesManager.Instance.RemoveValue(key);
        target.IsUnsetTarget();
    }

    private void OnInteractiveSphereDisappear()
    {
        _isInteractiveSphereDisappearing = true;
    }
}