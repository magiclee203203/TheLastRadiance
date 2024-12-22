public class PlayerAbilityCastState : PlayerBaseState
{
    private SOAbilityConfig _config;

    private void OnEnable()
    {
        _config = GameStateManager.Instance.ActiveAbilityConfig;

        foreach (var task in _config.CastStepOnEnableTasks)
        {
            task.Execute(this);
        }
    }

    private void OnDisable()
    {
        foreach (var task in _config.CastStepOnDisableTasks)
        {
            task.Execute(this);
        }
    }
}