using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[RequireComponent(typeof(InteractableFlashVFX))]
public class PressedPlate : SerializedMonoBehaviour, IKey
{
    [SerializeField, Required] private Transform _checkPoint;
    [SerializeField, Required] private List<ICommand> _tasksAfterActivated = new();
    [SerializeField, Required] private List<ICommand> _tasksAfterDeactivated = new();

    private InteractableFlashVFX _flashVFX;
    [SerializeField] private List<Collider> _pressers = new();
    private List<MeshRenderer> _meshRenderers;
    private List<Material> _currentMaterials;
    private bool _isActive;

    private void Awake()
    {
        _flashVFX = GetComponent<InteractableFlashVFX>();
        MaterialChanger.FindMeshRenderers(transform, out _meshRenderers, out _currentMaterials);
    }

    private void OnTriggerEnter(Collider other)
    {
        _flashVFX.StartFlash(InteractableType.PressedPlate, _meshRenderers);

        if (!_pressers.Contains(other))
            _pressers.Add(other);

        if (_isActive) return;
        Activate();
        _isActive = true;
    }

    private void OnTriggerExit(Collider other)
    {
        // in order to avoid a weird bug
        if (_pressers.Count == 0) return;

        if (_pressers.Contains(other))
            _pressers.Remove(other);

        if (_pressers.Count != 0) return;
        Deactivate();
        _flashVFX.StopFlash(_meshRenderers, _currentMaterials);
        _isActive = false;
    }

    public void Activate()
    {
        RunTasks(_tasksAfterActivated);
    }

    public void Deactivate()
    {
        RunTasks(_tasksAfterDeactivated);
    }

    private void RunTasks(List<ICommand> tasks)
    {
        foreach (var command in tasks)
        {
            command.Execute(this);
        }
    }
}