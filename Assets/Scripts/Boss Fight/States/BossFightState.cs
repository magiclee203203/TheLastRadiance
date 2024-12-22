public class BossFightState : BossBaseState
{
    public BossFightState(BossStateMachine context) : base(context)
    {
    }

    public override void OnEnterState()
    {
        context.BossAI.ExternalBehavior = context.BossFightCfg.BossFightAI;
        context.BossAI.RestartWhenComplete = true;
        context.BossAI.EnableBehavior();

        AudioManager.Instance.FadeInBGM(AudioManager.SoundType.BossFightBGM);
    }

    public override void OnExitState()
    {
        context.BossAI.DisableBehavior();
        context.BossAI.ExternalBehavior = null;

        AudioManager.Instance.FadeOutBGM(AudioManager.SoundType.BossFightBGM);
    }
}