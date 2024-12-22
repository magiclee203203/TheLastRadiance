using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputManager : MonoBehaviour
{
    [SerializeField, Required] private bool _useSEMGControl;

    [SerializeField, Required, AssetsOnly]
    [BoxGroup("Events Published"), LabelText("Input Value Change")]
    private SOPlayerInputValueChangedEvent _inputValueChangedEvent;

    [SerializeField, Required, AssetsOnly]
    [BoxGroup("Events Published"), LabelText("Switch Ability Pressed")]
    private SOEvent _switchAbilityPressedEvent;

    [SerializeField, Required, AssetsOnly]
    [BoxGroup("Events Published"), LabelText("Pause Pressed")]
    private SOEvent _pausePressedEvent;

    private PlayerInputActions _inputActions;
    private PlayerInputValue _inputValue;
    private UDPReceiver _udpReceiver;

    private void Awake()
    {
        _inputActions = new PlayerInputActions();

        _udpReceiver = GetComponent<UDPReceiver>();
        _udpReceiver.UseSEMGControl = _useSEMGControl;
        _udpReceiver.UDPCallback = OnReceiveUDPMessage;
    }

    private void OnEnable()
    {
        _inputActions.Enable();
        Subscribe();
    }

    private void OnDisable()
    {
        _inputActions.Disable();
        Unsubscribe();
    }

    private void OnChangeMoveVector(InputAction.CallbackContext ctx)
    {
        _inputValue.MoveVector = ctx.ReadValue<Vector2>();
        _inputValueChangedEvent.Notify(_inputValue);
    }

    private void OnChangeIsPressingUseAbility(InputAction.CallbackContext ctx)
    {
        _inputValue.IsPressingUseAbility = ctx.ReadValueAsButton();
        _inputValueChangedEvent.Notify(_inputValue);
    }

    private void OnChangeIsPressingRun(InputAction.CallbackContext ctx)
    {
        _inputValue.IsPressingRun = ctx.ReadValueAsButton();
        _inputValueChangedEvent.Notify(_inputValue);
    }

    private void OnPressSwitchAbility(InputAction.CallbackContext ctx)
    {
        _switchAbilityPressedEvent.Notify();
    }

    private void Subscribe()
    {
        _inputActions.Player.Move.performed += OnChangeMoveVector;
        _inputActions.Player.Move.canceled += OnChangeMoveVector;

        if (!_useSEMGControl)
        {
            _inputActions.Player.UseAbility.performed += OnChangeIsPressingUseAbility;
            _inputActions.Player.UseAbility.canceled += OnChangeIsPressingUseAbility;
        }

        _inputActions.Player.Run.performed += OnChangeIsPressingRun;
        _inputActions.Player.Run.canceled += OnChangeIsPressingRun;
        _inputActions.Player.SwitchAbility.started += OnPressSwitchAbility;
        _inputActions.Player.Pause.started += OnPressPause;
    }

    private void Unsubscribe()
    {
        _inputActions.Player.Move.performed -= OnChangeMoveVector;
        _inputActions.Player.Move.canceled -= OnChangeMoveVector;

        if (!_useSEMGControl)
        {
            _inputActions.Player.UseAbility.performed -= OnChangeIsPressingUseAbility;
            _inputActions.Player.UseAbility.canceled -= OnChangeIsPressingUseAbility;
        }

        _inputActions.Player.Run.performed -= OnChangeIsPressingRun;
        _inputActions.Player.Run.canceled -= OnChangeIsPressingRun;
        _inputActions.Player.SwitchAbility.started -= OnPressSwitchAbility;
        _inputActions.Player.Pause.started -= OnPressPause;
    }

    public void SetEnableState(bool enable)
    {
        if (enable)
            _inputActions.Enable();
        else
            _inputActions.Disable();
    }

    private void OnPressPause(InputAction.CallbackContext ctx)
    {
        _pausePressedEvent.Notify();
    }

    private void OnReceiveUDPMessage(string message)
    {
        switch (message)
        {
            case "0":
                _inputValue.IsPressingUseAbility = false;
                break;

            case "1":
                _inputValue.IsPressingUseAbility = true;
                break;

            default:
                return;
        }

        _inputValueChangedEvent.Notify(_inputValue);
    }
}

public struct PlayerInputValue
{
    public Vector2 MoveVector;
    public bool IsPressingUseAbility;
    public bool IsPressingRun;
}