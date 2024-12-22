using Sirenix.OdinInspector;
using UnityEngine;

public class OperateLockCommand : ICommand
{
    [SerializeField, Required, AssetsOnly] [BoxGroup("Events Published"), LabelText("Operate Lock")]
    private SOOperateLockEvent _operateLockEvent;

    [SerializeField, Required] private int _lockNum;

    public void Execute<T>(T receiver)
    {
        Debug.Log($"Operate Lock {_lockNum}！");
        _operateLockEvent.Notify(_lockNum);
    }
}