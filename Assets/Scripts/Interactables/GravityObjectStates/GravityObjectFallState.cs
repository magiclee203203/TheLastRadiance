using DG.Tweening;
using UnityEngine;

public class GravityObjectFallState : GravityObjectBaseState
{
    private GameObject _pressedPlateDestination;
    private bool _isFreeFalling;
    private float _freeFallingVelocity;
    private float _freeFallingHeight;

    public GravityObjectFallState(GravityObject context) : base(context)
    {
    }

    public override void OnEnterState()
    {
        context.IgnoreCollisionBetweenPlayer(true);

        _isFreeFalling = !HasPressedPlateUnderneath();

        if (!_isFreeFalling)
        {
            context.transform.DOMove(_pressedPlateDestination.transform.position, context.FlyToPressedPlateDuration)
                .SetEase(Ease.InQuad);
        }
        else
        {
            _freeFallingHeight = context.transform.position.y;
            _freeFallingVelocity = 0.0f;
        }
    }

    public override void Update()
    {
        if (!_isFreeFalling) return;
        FreeFalling();
    }

    private void FreeFalling()
    {
        var deltaTime = Time.deltaTime;
        _freeFallingVelocity += context.FreeFallingForce * deltaTime;
        _freeFallingHeight -= _freeFallingVelocity * deltaTime;
        context.transform.position =
            new Vector3(context.transform.position.x, _freeFallingHeight, context.transform.position.z);
    }

    private bool HasPressedPlateUnderneath()
    {
        var pressedPlateLayer = LayerMask.NameToLayer(VariableNamesDefine.PressedPlateLayerMaskName);

        var result = Physics.BoxCast(context.transform.position,
            context.PressedPlateCheckBoxSize * 0.5f,
            -context.transform.up,
            out RaycastHit hit,
            Quaternion.identity,
            context.PressedPlateCheckDistance,
            1 << pressedPlateLayer);

        if (result)
            _pressedPlateDestination = hit.collider.gameObject;

        return result;
    }
}