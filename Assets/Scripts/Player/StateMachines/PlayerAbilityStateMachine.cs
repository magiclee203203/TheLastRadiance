using Animancer.FSM;
using Sirenix.OdinInspector;
using UnityEngine;

public class PlayerAbilityStateMachine : MonoBehaviour
{
    [SerializeField, Required] [BoxGroup("States")]
    private PlayerBaseState _idle;

    [SerializeField, Required] [BoxGroup("States")]
    private PlayerBaseState _charge;

    [SerializeField, Required] [BoxGroup("States")]
    private PlayerBaseState _cast;

    [SerializeField, Required, AssetsOnly] [BoxGroup("Events Subscribed"), LabelText("Charge Animation End")]
    private SOEvent _chargeAnimationEndEvent;

    [SerializeField, Required, AssetsOnly] [BoxGroup("Events Subscribed"), LabelText("Cast Animation End")]
    private SOEvent _castAnimationEndEvent;

    [SerializeField, Required, AssetsOnly] [BoxGroup("Events Subscribed"), LabelText("Player Press Switch Ability")]
    private SOEvent _pressedSwitchAbilityEvent;

    [SerializeField, Required, AssetsOnly] [BoxGroup("Events Published"), LabelText("Change Active Ability")]
    private SOEvent _changeActiveAbilityEvent;

    private readonly StateMachine<PlayerBaseState>.WithDefault _stateMachine = new();

    private bool _isChargeAnimationEnd;
    private bool _isCastAnimationEnd;

    private void Start()
    {
        _stateMachine.DefaultState = _idle;
    }

    private void OnEnable()
    {
        _chargeAnimationEndEvent.Subscribe(OnChargeAnimationEnd);
        _castAnimationEndEvent.Subscribe(OnCastAnimationEnd);
        _pressedSwitchAbilityEvent.Subscribe(OnPressedSwitchAbility);
    }

    private void OnDisable()
    {
        _chargeAnimationEndEvent.Unsubscribe(OnChargeAnimationEnd);
        _castAnimationEndEvent.Unsubscribe(OnCastAnimationEnd);
        _pressedSwitchAbilityEvent.Unsubscribe(OnPressedSwitchAbility);
    }

    private void Update()
    {
        switch (_stateMachine.CurrentState)
        {
            case PlayerAbilityIdleState:
                _isChargeAnimationEnd = false;
                _isCastAnimationEnd = false;

                if (Player.Instance.InputValue.IsPressingUseAbility &&
                    GameStateManager.Instance.ActiveAbilityConfig != null)
                    _stateMachine.TrySetState(_charge);

                break;

            case PlayerAbilityChargeState:
                if (Player.Instance.InputValue.IsPressingUseAbility) break;
                _stateMachine.TrySetState(_isChargeAnimationEnd ? _cast : _idle);

                break;

            case PlayerAbilityCastState:
                if (_isCastAnimationEnd)
                    _stateMachine.ForceSetDefaultState();

                break;
        }
    }

    private void OnChargeAnimationEnd()
    {
        _isChargeAnimationEnd = true;
    }

    private void OnCastAnimationEnd()
    {
        _isCastAnimationEnd = true;
    }

    private void OnPressedSwitchAbility()
    {
        if (_stateMachine.CurrentState is PlayerAbilityIdleState)
            _changeActiveAbilityEvent.Notify();
    }
}