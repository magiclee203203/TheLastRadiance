using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[RequireComponent(typeof(InteractableFlashVFX))]
public abstract class EnergyBallReceiver : SerializedMonoBehaviour, IInteractable, IKey
{
    [SerializeField, Required] private List<ICommand> _tasksAfterActivated = new();

    private InteractableFlashVFX _flashVFX;
    protected List<MeshRenderer> meshRenderers;
    protected List<Material> currentMaterials;

    public GameObject Obj => gameObject;

    protected virtual void Awake()
    {
        _flashVFX = GetComponent<InteractableFlashVFX>();
        MaterialChanger.FindMeshRenderers(transform, out meshRenderers, out currentMaterials);
    }

    public void IsDetected()
    {
        _flashVFX.StartFlash(InteractableType.EnergyBallReceiver, meshRenderers);
    }

    public void IsUndetected()
    {
        _flashVFX.StopFlash(meshRenderers, currentMaterials);
    }

    public void IsSetTarget()
    {
    }

    public void IsUnsetTarget()
    {
    }

    protected virtual void OnCollisionEnter(Collision other)
    {
        if (!other.collider.CompareTag(VariableNamesDefine.EnergyTag) || !enabled) return;
        Activate();
    }

    public void Activate()
    {
        foreach (var command in _tasksAfterActivated)
        {
            command.Execute(this);
        }
    }

    public void Deactivate()
    {
    }
}