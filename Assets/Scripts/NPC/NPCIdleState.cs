using DG.Tweening;

public class NPCIdleState : NPCLocomotionBaseState
{
    private Tweener _tweener;

    public NPCIdleState(FollowAndArcMovement context) : base(context)
    {
    }

    public override void OnEnterState()
    {
        _tweener = context.transform.DOMoveY(context.IdleFloatOffset, context.IdleFloatDuration)
            .SetRelative().SetLoops(-1, LoopType.Yoyo);
    }

    public override void OnExitState()
    {
        _tweener.Kill();
        _tweener = null;
    }
}