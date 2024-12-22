public class BossIdleState : BossBaseState
{
    public BossIdleState(BossStateMachine context) : base(context)
    {
    }

    public override void OnEnterState()
    {
        context.Animancer.Play(context.BossFightCfg.IdleStateAnimation);
    }
}