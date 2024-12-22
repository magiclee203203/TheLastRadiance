using Sirenix.OdinInspector;
using Unity.Cinemachine;
using UnityEngine;

public class SetCameraPriorityCommand : ICommand
{
    [SerializeField, Required, SceneObjectsOnly]
    private CinemachineCamera _targetCamera;

    [SerializeField, Required] private int _priority;

    public void Execute<T>(T receiver)
    {
        _targetCamera.Priority = _priority;
    }
}