using Animancer;
using Animancer.FSM;
using BehaviorDesigner.Runtime;
using Sirenix.OdinInspector;
using UnityEngine;

public class BossStateMachine : MonoBehaviour
{
    [SerializeField, Required, AssetsOnly] private SOBossFightConfig _bossFightCfg;
    [SerializeField, Required] private int _bossHP = 4;

    [SerializeField, Required, AssetsOnly] [BoxGroup("Events Subscribed"), LabelText("Dialogue Ended")]
    private SODialogueTriggerEvent _dialogueEndedEvent;

    [SerializeField, Required, AssetsOnly] [BoxGroup("Events Subscribed"), LabelText("Is Hit")]
    private SOEvent _isHitEvent;

    [SerializeField, Required, AssetsOnly] [BoxGroup("Events Published"), LabelText("Dialogue Trigger")]
    private SODialogueTriggerEvent _triggerDialogueEvent;

    private readonly StateMachine<BossBaseState>.WithDefault _stateMachine = new();
    private BossBaseState _idle;
    private BossBaseState _standUp;
    private BossBaseState _fightState;
    private BossBaseState _defeated;

    private AnimancerComponent _animancer;
    private BehaviorTree _bossAI;
    private Rigidbody _rb;

    private int _currentHP;

    public StateMachine<BossBaseState>.WithDefault StateMachine => _stateMachine;
    public BossBaseState FightState => _fightState;
    public SOBossFightConfig BossFightCfg => _bossFightCfg;
    public AnimancerComponent Animancer => _animancer;
    public BehaviorTree BossAI => _bossAI;
    public Rigidbody Rb => _rb;
    public SODialogueTriggerEvent TriggerDialogueEvent => _triggerDialogueEvent;

    private void Awake()
    {
        _animancer = GetComponent<AnimancerComponent>();
        _bossAI = GetComponent<BehaviorTree>();
        _rb = GetComponent<Rigidbody>();

        InitStates();

        // init HP
        _currentHP = _bossHP;
    }

    private void OnEnable()
    {
        _dialogueEndedEvent.Subscribe(OnDialogueEnded);
        _isHitEvent.Subscribe(OnIsHit);
    }

    private void OnDisable()
    {
        _dialogueEndedEvent.Unsubscribe(OnDialogueEnded);
        _isHitEvent.Unsubscribe(OnIsHit);
    }

    private void Start()
    {
        _stateMachine.DefaultState = _idle;
        RegisterBodyParts();
    }

    private void OnDialogueEnded(string conversationTitle)
    {
        if (conversationTitle != VariableNamesDefine.EnterBossRoomConversationTitle) return;
        _stateMachine.TrySetState(_standUp);
    }

    private void OnIsHit()
    {
        _currentHP--;
        if (_currentHP <= 0)
            _stateMachine.TrySetState(_defeated);
    }

    private void InitStates()
    {
        _idle = new BossIdleState(this);
        _standUp = new BossStandUpState(this);
        _fightState = new BossFightState(this);
        _defeated = new BossDefeatedState(this);
    }

    private void RegisterBodyParts()
    {
        GlobalVariablesManager.Instance.SetValue(VariableNamesDefine.BossCollider, GetComponent<CapsuleCollider>());

        var bossWeapon = GameObject.Find(VariableNamesDefine.BossWeaponObject);
        GlobalVariablesManager.Instance.SetValue(VariableNamesDefine.BossWeaponObject, bossWeapon);

        var flashedArm = GameObject.Find(VariableNamesDefine.BossFlashedArm);
        GlobalVariablesManager.Instance.SetValue(VariableNamesDefine.BossFlashedArm, flashedArm);

        var flashedHand = GameObject.Find(VariableNamesDefine.BossFlashedHand);
        GlobalVariablesManager.Instance.SetValue(VariableNamesDefine.BossFlashedHand, flashedHand);

        var laserShootPoint = GameObject.Find(VariableNamesDefine.BossLaserShootPoint);
        GlobalVariablesManager.Instance.SetValue(VariableNamesDefine.BossLaserShootPoint, laserShootPoint);
    }
}