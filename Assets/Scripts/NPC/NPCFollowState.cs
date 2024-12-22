using DG.Tweening;

public class NPCFollowState : NPCLocomotionBaseState
{
    public NPCFollowState(FollowAndArcMovement context) : base(context)
    {
    }

    private Tweener _tweener;

    public override void OnEnterState()
    {
        _tweener = context.transform.DOMove(context.FollowStartPosition, context.FollowDuration);
    }

    public override void LateUpdate()
    {
        _tweener.ChangeValues(context.transform.position, context.FollowStartPosition);
    }

    public override void OnExitState()
    {
        _tweener.Kill();
        _tweener = null;
    }
}