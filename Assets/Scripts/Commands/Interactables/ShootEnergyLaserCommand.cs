using System.Collections;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

public class ShootEnergyLaserCommand : ICommand
{
    [SerializeField, Required] private float _chargingDuration = 2.0f;
    [SerializeField, Required, AssetsOnly] private GameObject _chargingVFX;
    [SerializeField, Required, AssetsOnly] private GameObject _laser;
    [SerializeField, Required] private float _shootShakeDuration = 0.1f;
    [SerializeField, Required] private float _shootShakeForce = 1.0f;

    private EnergyLaserTurret _turret;
    private bool _isInProgress;

    public void Execute<T>(T receiver)
    {
        if (receiver is not EnergyLaserTurret turret) return;
        _turret = turret;

        if (_isInProgress) return;
        _isInProgress = true;
        _turret.StartCoroutine(ChargeForShoot(_chargingDuration));
    }

    private IEnumerator ChargeForShoot(float duration)
    {
        AudioManager.Instance.Play(AudioManager.SoundType.TurretCharge);

        var chargingVFX = Object.Instantiate(_chargingVFX,
            _turret.ChargingPoint.position, Quaternion.identity, _turret.transform);
        Object.Destroy(chargingVFX, duration);

        yield return new WaitForSeconds(duration);
        AudioManager.Instance.Stop(AudioManager.SoundType.TurretCharge);

        // Shake
        _turret.Gun.DOShakePosition(_shootShakeDuration, new Vector3(0.0f, 0.0f, _shootShakeForce));

        // Generate laser
        var laser = Object.Instantiate(_laser, _turret.ShootingPoint.position,
            _turret.ShootingPoint.rotation * Quaternion.Euler(0.0f, 180.0f, 0.0f));

        AudioManager.Instance.Play(AudioManager.SoundType.ShootEnergyBall);

        // handle collision
        IgnoreCollision(laser);

        _isInProgress = false;
    }

    private void IgnoreCollision(GameObject laser)
    {
        var turretColliders = _turret.GetComponentsInChildren<Collider>();
        var laserCollider = laser.GetComponent<Collider>();

        foreach (var collider in turretColliders)
        {
            Physics.IgnoreCollision(collider, laserCollider, true);
        }
    }
}