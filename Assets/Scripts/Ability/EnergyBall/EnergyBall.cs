using Sirenix.OdinInspector;
using UnityEngine;

public class EnergyBall : MonoBehaviour
{
    [SerializeField, Required, AssetsOnly] [BoxGroup("Prefabs")]
    private GameObject _impactParticle;

    [SerializeField, Required, AssetsOnly] [BoxGroup("Prefabs")]
    private GameObject _projectileParticle;

    [SerializeField, Required, AssetsOnly] [BoxGroup("Prefabs")]
    private GameObject _muzzleParticle;

    [SerializeField, Required] private float _flyForce;
    [SerializeField, Required] private float _maxFlyDistance;

    private Rigidbody _rb;
    private SphereCollider _collider;

    private Vector3 _spawnPoint;
    private Vector3 _ballFlyingDirection;
    private float _collisionDetectionDistance;
    private GameObject _targetSwitch;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _collider = GetComponent<SphereCollider>();
        _spawnPoint = transform.position;
    }

    private void Start()
    {
        GlobalVariablesManager.Instance.GetValue(VariableNamesDefine.TargetEnergyBallReceiver, out _targetSwitch);
        IgnoreCollision();
        GenerateVFX();
        Fly();
    }

    private void FixedUpdate()
    {
        if (_targetSwitch == null)
            HandleSelfDestroy();
    }

    private void Fly()
    {
        Vector3 flyDir;

        if (_targetSwitch == null)
        {
            flyDir = transform.forward;
        }
        else
        {
            var hitPoint = FindHitPoint();

            if (hitPoint == null)
            {
                Debug.LogError("HitPoint Not Found");
                return;
            }

            flyDir = (hitPoint.position - transform.position).normalized;
        }

        _rb.AddForce(flyDir * _flyForce);
    }

    private Transform FindHitPoint()
    {
        var nodes = _targetSwitch.GetComponentsInChildren<Transform>();
        foreach (var node in nodes)
        {
            if (node.name == VariableNamesDefine.EnergyBallReceiverHitPoint)
                return node;
        }

        return null;
    }

    private void HandleSelfDestroy()
    {
        // Calculate current distance from spawn point
        var distance = Vector3.Distance(transform.position, _spawnPoint);
        if (distance >= _maxFlyDistance)
            ExecuteDestroyProcess(false);
    }

    private void OnCollisionEnter()
    {
        ExecuteDestroyProcess(true);
    }

    private void ExecuteDestroyProcess(bool collisionDetected)
    {
        Destroy(gameObject);
        Destroy(_projectileParticle, 3f);

        if (!collisionDetected) return;
        Instantiate(_impactParticle, transform.position, Quaternion.identity);
        AudioManager.Instance.Play(AudioManager.SoundType.EnergyBallExplosion);
    }

    private void IgnoreCollision()
    {
        if (!GlobalVariablesManager.Instance.GetValue(VariableNamesDefine.PlayerCollider,
                out CapsuleCollider playerCollider))
        {
            Debug.LogError("Player Collider Not Found!");
            return;
        }

        if (!GlobalVariablesManager.Instance.GetValue(VariableNamesDefine.BossCollider,
                out CapsuleCollider bossCollider))
        {
            Debug.LogError("Player Collider Not Found!");
            return;
        }

        Physics.IgnoreCollision(_collider, playerCollider, true);
        Physics.IgnoreCollision(_collider, bossCollider, true);
    }

    private void GenerateVFX()
    {
        _projectileParticle = Instantiate(_projectileParticle, transform.position, transform.rotation);
        _projectileParticle.transform.parent = transform;

        _muzzleParticle = Instantiate(_muzzleParticle, transform.position,
            transform.rotation * Quaternion.Euler(0.0f, 180.0f, 0.0f));
        _muzzleParticle.transform.parent = transform;
        Destroy(_muzzleParticle, 1.5f);
    }
}