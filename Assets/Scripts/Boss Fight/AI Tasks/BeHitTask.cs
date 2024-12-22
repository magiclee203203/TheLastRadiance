using Animancer;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

[TaskCategory("Custom"), TaskName("Boss Is Hit")]
public class BeHitTask : Action
{
    [SerializeField] private SOBossFightConfig _bossFightConfig;
    [SerializeField] private SOEvent _isHitEvent;

    private AnimancerComponent _animancer;
    private bool _animationEnd;

    public override void OnAwake()
    {
        _animancer = GetComponent<AnimancerComponent>();
    }

    public override void OnStart()
    {
        _animationEnd = false;

        var state = _animancer.Play(_bossFightConfig.BeHitAnimation);
        state.Events(this).OnEnd ??= () => { _animationEnd = true; };
    }

    public override TaskStatus OnUpdate()
    {
        return _animationEnd ? TaskStatus.Success : TaskStatus.Running;
    }

    public override void OnEnd()
    {
        _isHitEvent.Notify();
    }
}