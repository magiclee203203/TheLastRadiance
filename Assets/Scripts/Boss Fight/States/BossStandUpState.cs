public class BossStandUpState : BossBaseState
{
    public BossStandUpState(BossStateMachine context) : base(context)
    {
    }

    public override void OnEnterState()
    {
        var state = context.Animancer.Play(context.BossFightCfg.StandUpAnimation);
        state.Events(this).OnEnd ??= () => { context.StateMachine.TrySetState(context.FightState); };

        AudioManager.Instance.Play(AudioManager.SoundType.BossStandUp);
    }
}