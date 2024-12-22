using DG.Tweening;
using UnityEngine;

public class GravityObjectElevateState : GravityObjectBaseState
{
    private Tweener _tweener;

    private Vector3 MoveEndPoint
    {
        get
        {
            var handForward = -context.PlayerLeftHand.transform.right;
            handForward.y = 0.0f;
            return context.PlayerLeftHand.transform.position + handForward.normalized * context.PlayerLeftHandOffset.z +
                   Vector3.up * context.PlayerLeftHandOffset.y;
        }
    }

    public GravityObjectElevateState(GravityObject context) : base(context)
    {
    }

    public override void OnEnterState()
    {
        context.IgnoreCollisionBetweenPlayer(true);
    }

    public override void OnExitState()
    {
        _tweener?.Kill();
        _tweener = null;
    }

    public override void LateUpdate()
    {
        if (_tweener == null)
            _tweener = context.transform.DOMove(MoveEndPoint, context.ElevateDuration);
        else
            _tweener.ChangeEndValue(MoveEndPoint, true);
    }
}