using UnityEngine;

public interface IInteractable
{
    public GameObject Obj { get; }

    void IsDetected();
    void IsUndetected();

    void IsSetTarget();
    void IsUnsetTarget();
}

public enum InteractableType
{
    EnergyBallReceiver,
    GravityObject,
    PressedPlate
}