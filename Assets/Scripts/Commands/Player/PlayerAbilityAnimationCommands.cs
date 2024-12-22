using Animancer;

public class PlayerAbilityPlayChargeAnimationCommand : ICommand
{
    public virtual void Execute<T>(T receiver)
    {
        var cfg = GameStateManager.Instance.ActiveAbilityConfig;

        var state = Player.Instance.AbilityLayer.Play(cfg.ChargeAnimation);
        state.Events(this).OnEnd ??= () => { cfg.ChargeAnimationEndEvent.Notify(); };
    }
}

public class PlayerAbilityFadeOutAnimationCommand : ICommand
{
    public void Execute<T>(T receiver)
    {
        Player.Instance.AbilityLayer.StartFade(0.0f);
    }
}

public class PlayerAbilityPlayCastAnimationCommand : ICommand
{
    private static readonly StringReference
        _animationMidwayEventName = VariableNamesDefine.CastAnimationMidwayEventName;

    public virtual void Execute<T>(T receiver)
    {
        var cfg = GameStateManager.Instance.ActiveAbilityConfig;

        var state = Player.Instance.AbilityLayer.Play(cfg.CastAnimation);
        state.Events(this).OnEnd ??= () => { cfg.CastAnimationEndEvent.Notify(); };
        state.Events(this).SetCallback(_animationMidwayEventName, () => { cfg.CastMidwayEvent.Notify(); });
    }
}