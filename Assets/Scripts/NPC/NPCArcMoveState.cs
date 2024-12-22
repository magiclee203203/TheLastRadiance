using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class NPCArcMoveState : NPCLocomotionBaseState
{
    public NPCArcMoveState(FollowAndArcMovement context) : base(context)
    {
    }

    private Tweener _moveTweener;
    private Tweener _circularTweener;
    private Tweener _floatTweener;

    public override void OnEnterState()
    {
        var waypoints = CalculateWaypoints();
        _moveTweener = context.transform.DOMove(waypoints[0], 0.01f)
            .SetEase(Ease.Flash)
            .OnComplete(() => { CircularMove(waypoints); });
    }

    private void CircularMove(List<Vector3> waypoints)
    {
        _circularTweener = context.transform.DOPath(waypoints.ToArray(), context.ArcMoveDuration, PathType.CatmullRom)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.Linear);

        _floatTweener = context.transform.DOMoveY(context.FloatOffset, context.FloatDuration)
            .SetRelative()
            .SetLoops(-1, LoopType.Yoyo);
    }

    public override void OnExitState()
    {
        _moveTweener?.Kill();
        _circularTweener?.Kill();
        _floatTweener?.Kill();

        _moveTweener = null;
        _circularTweener = null;
        _floatTweener = null;
    }

    private List<Vector3> CalculateWaypoints()
    {
        var waypoints = new List<Vector3>();

        var startRot = Quaternion.AngleAxis(context.FollowAngleRange, Player.Instance.transform.up);
        var startDir = startRot * Player.Instance.transform.right;

        var angleIncrement = context.FollowAngleRange * 2.0f / context.ArcSegments;
        for (var i = 0; i < context.ArcSegments; i++)
        {
            var angle = -angleIncrement * i;
            var rotation = Quaternion.AngleAxis(angle, Player.Instance.transform.up);
            var dir = rotation * startDir;
            var point = Player.Instance.transform.position + dir * context.FollowDistance +
                        Vector3.up * context.HeightOffset;

            waypoints.Add(point);
        }

        return waypoints;
    }
}