public class GravityObjectStaticState : GravityObjectBaseState
{
    public GravityObjectStaticState(GravityObject context) : base(context)
    {
    }

    public override void OnEnterState()
    {
        context.IgnoreCollisionBetweenPlayer(false);
    }
}