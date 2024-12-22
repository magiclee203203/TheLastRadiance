using Animancer.FSM;

public abstract class NPCLocomotionBaseState : State
{
    protected FollowAndArcMovement context;

    protected NPCLocomotionBaseState(FollowAndArcMovement context)
    {
        this.context = context;
    }

    public virtual void LateUpdate()
    {
    }
}