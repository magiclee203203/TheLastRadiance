using System.Collections.Generic;
using Animancer.FSM;
using Sirenix.OdinInspector;
using UnityEngine;

public class BattleField : MonoBehaviour
{
    [SerializeField, Required, AssetsOnly] private SOBossFightConfig _bossFightCfg;

    [SerializeField, Required, AssetsOnly] [BoxGroup("Events Subscribed"), LabelText("Hit By Boss Laser")]
    private SOEvent _hitByBossLaserEvent;

    [SerializeField, Required, AssetsOnly] [BoxGroup("Events Subscribed"), LabelText("Fight Turret Activate")]
    private SOEvent _fightTurretActivateEvent;

    [SerializeField, Required, SceneObjectsOnly]
    private List<Transform> _spawnPoints = new();

    private StateMachine<BattleFieldBaseState>.WithDefault _stateMachine = new();
    private BattleFieldIdleState _idle;
    private BattleFieldSpawnState _spawn;

    public SOBossFightConfig BossFightCfg => _bossFightCfg;
    public List<Transform> SpawnPoints => _spawnPoints;

    private void Awake()
    {
        InitStates();
    }

    private void OnEnable()
    {
        _hitByBossLaserEvent.Subscribe(OnHitByBossLaser);
        _fightTurretActivateEvent.Subscribe(OnFightTurretActivate);
    }

    private void OnDisable()
    {
        _hitByBossLaserEvent.Unsubscribe(OnHitByBossLaser);
        _fightTurretActivateEvent.Unsubscribe(OnFightTurretActivate);
    }

    private void Start()
    {
        _stateMachine.DefaultState = _idle;
        _spawn.StateEndCallback = () => { _stateMachine.TrySetState(_idle); };
    }

    private void InitStates()
    {
        _idle = new BattleFieldIdleState(this);
        _spawn = new BattleFieldSpawnState(this);
    }

    private void OnHitByBossLaser()
    {
        _stateMachine.TrySetState(_spawn);
    }

    private void OnFightTurretActivate()
    {
        _spawn.TurretActivate();
    }
}