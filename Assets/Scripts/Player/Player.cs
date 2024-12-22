using Animancer;
using Sirenix.OdinInspector;
using UnityEngine;

public class Player : Singleton<Player>
{
    [SerializeField, Required, AssetsOnly] private AvatarMask _upperBodyMask;

    [SerializeField, Required, AssetsOnly]
    [BoxGroup("Events Subscribe")]
    private SOPlayerInputValueChangedEvent _inputValueChangedEvent;

    private PlayerInputValue _inputValue;
    private AnimancerComponent _animancer;
    private AnimancerLayer _locomotionLayer;
    private AnimancerLayer _abilityLayer;
    private Rigidbody _rb;
    private PlayerInputManager _inputManager;

    # region Singleton Properties

    public PlayerInputValue InputValue => _inputValue;
    public AnimancerComponent Animancer => _animancer;
    public AnimancerLayer LocomotionLayer => _locomotionLayer;
    public AnimancerLayer AbilityLayer => _abilityLayer;
    public Rigidbody Rb => _rb;
    public PlayerInputManager InputManager => _inputManager;

    # endregion

    private void Awake()
    {
        _animancer = GetComponentInChildren<AnimancerComponent>();
        _locomotionLayer = _animancer.Layers[0];
        _abilityLayer = _animancer.Layers[1];
        _abilityLayer.Mask = _upperBodyMask;

        _rb = GetComponent<Rigidbody>();
        _inputManager = GetComponentInChildren<PlayerInputManager>();
    }

    private void Start()
    {
        // Register Global Variables
        RegisterGlobalVariables();
    }

    private void OnEnable()
    {
        _inputValueChangedEvent.Subscribe(OnInputValueChanged);
    }

    private void OnDisable()
    {
        _inputValueChangedEvent.Unsubscribe(OnInputValueChanged);
    }

    private void OnInputValueChanged(PlayerInputValue newValue)
    {
        _inputValue = newValue;
    }

    private void RegisterGlobalVariables()
    {
        var leftHandGameObj = GameObject.Find(VariableNamesDefine.PlayerLeftHand);
        GlobalVariablesManager.Instance.SetValue(VariableNamesDefine.PlayerLeftHand, leftHandGameObj);

        var playerHitPointGameObj = GameObject.Find(VariableNamesDefine.PlayerHitPointGameObject);
        GlobalVariablesManager.Instance.SetValue(VariableNamesDefine.PlayerHitPointGameObject, playerHitPointGameObj);

        var playerColliderGameObj = GameObject.Find(VariableNamesDefine.PlayerColliderGameObject);
        GlobalVariablesManager.Instance.SetValue(VariableNamesDefine.PlayerCollider,
            playerColliderGameObj.GetComponent<CapsuleCollider>());
    }
}