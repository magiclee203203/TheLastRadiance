using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

[TaskCategory("Custom"), TaskName("Boss Is Hit")]
public class IsHitCondition : Conditional
{
    private bool _isHit;

    public override void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.TryGetComponent(out Laser laser)) return;
        _isHit = laser.LaserSource == Laser.Source.Turret;
    }

    public override TaskStatus OnUpdate()
    {
        return _isHit ? TaskStatus.Success : TaskStatus.Failure;
    }

    public override void OnEnd()
    {
        _isHit = false;
    }
}