using Animancer.FSM;
using Sirenix.OdinInspector;
using UnityEngine;

public class FollowAndArcMovement : MonoBehaviour
{
    [SerializeField, Required] private float _heightOffset = 0.9f;
    [SerializeField, Required] private float _rotateSpeed = 10.0f;

    [SerializeField, Required] [BoxGroup("Idle Properties"), LabelText("Float Offset")]
    private float _idleFloatOffset = 0.2f;

    [SerializeField, Required] [BoxGroup("Idle Properties"), LabelText("Float Duration")]
    private float _idleFloatDuration = 0.8f;

    [SerializeField, Required] [BoxGroup("Follow Properties")]
    private float _followDistance = 2.0f;

    [SerializeField, Required] [BoxGroup("Follow Properties")]
    private float _followAngleRange = 45.0f;

    [SerializeField, Required] [BoxGroup("Follow Properties")]
    private float _followDuration = 1.0f;

    [SerializeField, Required] [BoxGroup("Follow Properties")]
    private float _distanceDetectError = 0.1f;

    [SerializeField, Required] [BoxGroup("Arc Move Properties")]
    private int _arcSegments = 20;

    [SerializeField, Required] [BoxGroup("Arc Move Properties")]
    private float _arcMoveDuration = 3.0f;

    [SerializeField, Required] [BoxGroup("Arc Move Properties")]
    private float _floatOffset = 0.1f;

    [SerializeField, Required] [BoxGroup("Arc Move Properties")]
    private float _floatDuration = 0.6f;

    private readonly StateMachine<NPCLocomotionBaseState>.WithDefault _stateMachine = new();
    private NPCLocomotionBaseState _idle;
    private NPCLocomotionBaseState _follow;
    private NPCLocomotionBaseState _arcMove;

    public Vector3 FollowStartPosition
    {
        get
        {
            var rotation = Quaternion.AngleAxis(_followAngleRange, Player.Instance.transform.up);
            var targetDir = rotation * Player.Instance.transform.right;
            return Player.Instance.transform.position + targetDir * _followDistance + Vector3.up * _heightOffset;
        }
    }

    public float HeightOffset => _heightOffset;
    public float IdleFloatOffset => _idleFloatOffset;
    public float IdleFloatDuration => _idleFloatDuration;
    public float FollowDistance => _followDistance;
    public float FollowDuration => _followDuration;
    public float FollowAngleRange => _followAngleRange;
    public int ArcSegments => _arcSegments;
    public float ArcMoveDuration => _arcMoveDuration;
    public float FloatOffset => _floatOffset;
    public float FloatDuration => _floatDuration;

    private void Awake()
    {
        _idle = new NPCIdleState(this);
        _follow = new NPCFollowState(this);
        _arcMove = new NPCArcMoveState(this);
    }

    private void Start()
    {
        _stateMachine.DefaultState = _idle;
    }

    private void Update()
    {
        if (_stateMachine.CurrentState is NPCIdleState) return;
        Rotate();
    }

    private void LateUpdate()
    {
        CheckState();
        _stateMachine.CurrentState.LateUpdate();
    }

    private void CheckState()
    {
        switch (_stateMachine.CurrentState)
        {
            case NPCIdleState:
                if (Player.Instance.InputValue.MoveVector != Vector2.zero && GameStateManager.Instance.IsNPCActive)
                    _stateMachine.TrySetState(_follow);

                break;

            case NPCFollowState:
                if (Player.Instance.InputValue.MoveVector != Vector2.zero) break;

                if (IsInValidRange())
                    _stateMachine.TrySetState(_arcMove);

                break;

            case NPCArcMoveState:
                if (Player.Instance.InputValue.MoveVector != Vector2.zero)
                    _stateMachine.TrySetState(_follow);

                break;
        }
    }

    private void Rotate()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, Player.Instance.transform.rotation,
            _rotateSpeed * Time.deltaTime);
    }

    private bool IsInValidRange()
    {
        var sqrDistance = (transform.position - FollowStartPosition).sqrMagnitude;
        return sqrDistance <= _distanceDetectError * _distanceDetectError;
    }
}