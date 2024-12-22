using Animancer;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

[TaskCategory("Custom"), TaskName("Boss Shoot")]
public class ShootTask : Action
{
    [SerializeField] private SOBossFightConfig _bossFightConfig;
    [SerializeField] private StringAsset _shootEvent;
    [SerializeField] private GameObject _laserPrefab;

    private AnimancerComponent _animancer;
    private bool _animationEnd;
    private Transform _laserSpawnPoint;
    private GameObject _playerHitPoint;

    public override void OnAwake()
    {
        _animancer = GetComponent<AnimancerComponent>();
    }

    public override void OnStart()
    {
        _animationEnd = false;

        HandleTransformInfo();
        HandleAnimation();

        AudioManager.Instance.Play(AudioManager.SoundType.BossShoot);
    }

    public override TaskStatus OnUpdate()
    {
        return _animationEnd ? TaskStatus.Success : TaskStatus.Running;
    }

    private void ShootLaser()
    {
        var laserDir = _playerHitPoint.transform.position - _laserSpawnPoint.position;
        var laserRotation = Quaternion.LookRotation(laserDir) * Quaternion.Euler(0.0f, 180.0f, 0.0f);
        Object.Instantiate(_laserPrefab, _laserSpawnPoint.position, laserRotation);
    }

    private void HandleTransformInfo()
    {
        if (!GlobalVariablesManager.Instance.GetValue(VariableNamesDefine.BossLaserShootPoint,
                out GameObject laserShootPoint))
            Debug.LogError("Boss Laser Point Not Found");
        _laserSpawnPoint = laserShootPoint.transform;

        if (!GlobalVariablesManager.Instance.GetValue(VariableNamesDefine.PlayerHitPointGameObject,
                out _playerHitPoint))
            Debug.LogError("Player Hit Point Not Found");
    }

    private void HandleAnimation()
    {
        var state = _animancer.Play(_bossFightConfig.ShootAnimation);
        state.Events(this).OnEnd ??= () => { _animationEnd = true; };
        state.Events(this).SetCallback(_shootEvent.Name, ShootLaser);
    }
}