using Animancer.FSM;

public abstract class BattleFieldBaseState : State
{
    protected BattleField context;

    protected BattleFieldBaseState(BattleField context)
    {
        this.context = context;
    }

    public virtual void Update()
    {
    }
}