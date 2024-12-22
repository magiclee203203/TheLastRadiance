using System.Collections.Generic;
using Animancer.FSM;
using Sirenix.OdinInspector;
using UnityEngine;

[RequireComponent(typeof(InteractableFlashVFX))]
public class GravityObject : MonoBehaviour, IInteractable
{
    [SerializeField, Required] [InfoBox("Distance between this object and player left hand when elevate")]
    private Vector3 _playerLeftHandOffset;

    [SerializeField, Required] private float _elevateDuration = 1.0f;

    [InfoBox("Cast Box To Detect Pressed Plate")]
    [SerializeField, Required, BoxGroup("Pressed Plate Check"), LabelText("Check Box Size")]
    private Vector3 _pressedPlateCheckBoxSize;

    [SerializeField, Required, BoxGroup("Pressed Plate Check"), LabelText("Check Distance")]
    private float _pressedPlateCheckDistance;

    [SerializeField, Required] private float _freeFallingForce = 19.6f;
    [SerializeField, Required] private float _flyToPressedPlateDuration = 0.3f;

    private readonly StateMachine<GravityObjectBaseState>.WithDefault _stateMachine = new();
    private GravityObjectBaseState _staticState;
    private GravityObjectBaseState _elevateState;
    private GravityObjectBaseState _fallState;

    private Collider _collider;
    private CapsuleCollider _playerCollider;
    private GameObject _playerLeftHand;
    private InteractableFlashVFX _flashVFX;
    private List<MeshRenderer> _meshRenderers;
    private List<Material> _currentMaterials;

    public GameObject Obj => gameObject;
    public Vector3 PlayerLeftHandOffset => _playerLeftHandOffset;
    public float ElevateDuration => _elevateDuration;
    public GameObject PlayerLeftHand => _playerLeftHand;
    public Vector3 PressedPlateCheckBoxSize => _pressedPlateCheckBoxSize;
    public float PressedPlateCheckDistance => _pressedPlateCheckDistance;
    public float FreeFallingForce => _freeFallingForce;
    public float FlyToPressedPlateDuration => _flyToPressedPlateDuration;

    private void Awake()
    {
        _collider = GetComponent<Collider>();
        _flashVFX = GetComponent<InteractableFlashVFX>();
        MaterialChanger.FindMeshRenderers(transform, out _meshRenderers, out _currentMaterials);

        InitStates();
    }

    private void Start()
    {
        // get player collider
        if (!GlobalVariablesManager.Instance.GetValue(VariableNamesDefine.PlayerCollider, out _playerCollider))
            Debug.LogError("Player Collider Not Found");

        // get player left hand
        if (!GlobalVariablesManager.Instance.GetValue(VariableNamesDefine.PlayerLeftHand,
                out _playerLeftHand))
            Debug.LogError("Player Left Hand Not Found");

        _stateMachine.DefaultState = _staticState;
    }

    // this can happen when player drop the object to the dark
    private void OnDisable()
    {
        _stateMachine.TrySetState(_staticState);
    }

    private void Update()
    {
        _stateMachine.CurrentState.Update();
    }

    private void LateUpdate()
    {
        _stateMachine.CurrentState.LateUpdate();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!enabled) return;
        if (!other.CompareTag(VariableNamesDefine.GroundTag)) return;
        _stateMachine.TrySetState(_staticState);
    }

    public void IsSetTarget()
    {
        Debug.Log($"{gameObject.name} Is set target");
        _stateMachine.TrySetState(_elevateState);
    }

    public void IsUnsetTarget()
    {
        Debug.Log($"{gameObject.name} Is unset target");
        _stateMachine.TrySetState(_fallState);
    }

    public void IsDetected()
    {
        _flashVFX.StartFlash(InteractableType.GravityObject, _meshRenderers);
    }

    public void IsUndetected()
    {
        _flashVFX.StopFlash(_meshRenderers, _currentMaterials);
    }

    public void IgnoreCollisionBetweenPlayer(bool ignore)
    {
        Physics.IgnoreCollision(_collider, _playerCollider, ignore);
    }

    private void InitStates()
    {
        _staticState = new GravityObjectStaticState(this);
        _elevateState = new GravityObjectElevateState(this);
        _fallState = new GravityObjectFallState(this);
    }
}