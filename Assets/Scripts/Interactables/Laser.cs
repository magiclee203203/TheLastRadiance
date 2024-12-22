using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class Laser : SerializedMonoBehaviour
{
    public enum Source
    {
        Turret,
        Boss
    }

    [SerializeField, Required] private float _flyForce = 100.0f;
    [SerializeField, Required, AssetsOnly] private GameObject _hitVFX;

    [SerializeField, Required] [BoxGroup("Collision"), LabelText("Ignore Player")]
    private bool _ignoreCollisionBetweenPlayer;

    [SerializeField, Required] [BoxGroup("Collision"), LabelText("Ignore Boss")]
    private bool _ignoreCollisionBetweenBoss;

    [SerializeField, Required, EnumToggleButtons]
    private Source _source = Source.Turret;

    [SerializeField] private List<ICommand> _tasksWhenCollision = new();

    private Rigidbody _rb;
    private Collider _collider;

    public Source LaserSource => _source;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _collider = GetComponent<Collider>();
    }

    private void Start()
    {
        HandleCollision();
        Fly();
    }

    private void Fly()
    {
        _rb.AddForce(-transform.forward * _flyForce, ForceMode.Force);
    }

    private void OnCollisionEnter(Collision other)
    {
        var contactPoint = other.GetContact(0);
        Instantiate(_hitVFX, contactPoint.point, Quaternion.identity);

        AudioManager.Instance.Play(AudioManager.SoundType.EnergyBallExplosion);

        foreach (var task in _tasksWhenCollision)
        {
            task.Execute(other);
        }

        Destroy(gameObject);
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    private void HandleCollision()
    {
        if (_ignoreCollisionBetweenPlayer)
        {
            if (!GlobalVariablesManager.Instance.GetValue(VariableNamesDefine.PlayerCollider,
                    out CapsuleCollider playerCollider))
                Debug.LogError("Player Collider Not Found");

            Physics.IgnoreCollision(_collider, playerCollider, true);
        }

        if (_ignoreCollisionBetweenBoss)
        {
            if (!GlobalVariablesManager.Instance.GetValue(VariableNamesDefine.BossCollider,
                    out CapsuleCollider bossCollider))
                Debug.LogError("Player Collider Not Found");

            Physics.IgnoreCollision(_collider, bossCollider, true);
        }
    }
}