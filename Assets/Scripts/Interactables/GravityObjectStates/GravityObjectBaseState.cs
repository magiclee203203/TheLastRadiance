using Animancer.FSM;

public abstract class GravityObjectBaseState : State
{
    protected readonly GravityObject context;

    protected GravityObjectBaseState(GravityObject context)
    {
        this.context = context;
    }

    public virtual void Update()
    {
    }

    public virtual void LateUpdate()
    {
    }
}