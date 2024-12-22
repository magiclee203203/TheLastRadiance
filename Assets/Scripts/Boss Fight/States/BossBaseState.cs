using Animancer.FSM;

public abstract class BossBaseState : State
{
    protected BossStateMachine context;

    protected BossBaseState(BossStateMachine context)
    {
        this.context = context;
    }
}