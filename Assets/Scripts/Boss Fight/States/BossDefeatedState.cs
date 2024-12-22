public class BossDefeatedState : BossBaseState
{
    public BossDefeatedState(BossStateMachine context) : base(context)
    {
    }

    public override void OnEnterState()
    {
        context.Rb.detectCollisions = false;

        var state = context.Animancer.Play(context.BossFightCfg.DefeatedAnimation);
        state.Events(this).OnEnd ??= () =>
        {
            state.IsPlaying = false;
            context.TriggerDialogueEvent.Notify(VariableNamesDefine.BossDefeatedConversationTitle);
        };

        AudioManager.Instance.Play(AudioManager.SoundType.BossDefeated);
    }
}