using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

public class EnergyLaserTurret : EnergyBallReceiver, IHidden
{
    [SerializeField, Required] private bool _initShow;
    [SerializeField, Required] private float _transitionDuration = 1.0f;
    [SerializeField] private List<ICommand> _tasksAfterShow = new();

    private Transform _chargingPoint;
    private Transform _shootingPoint;
    private Transform _gun;
    private List<Collider> _colliders = new();
    private Sequence _seq;

    public Transform ChargingPoint => _chargingPoint;
    public Transform ShootingPoint => _shootingPoint;
    public Transform Gun => _gun;

    public bool CurrentShow { get; set; }

    protected override void Awake()
    {
        base.Awake();

        _chargingPoint = transform.Find(VariableNamesDefine.LaserTurretChargingPoint);
        _shootingPoint = transform.Find(VariableNamesDefine.LaserTurretShootingPoint);
        _gun = transform.Find(VariableNamesDefine.LaserTurretGun);
        _colliders = GetComponentsInChildren<Collider>().ToList();

        if (!_initShow)
            HandleInitHide();
    }

    private void OnDisable()
    {
        _seq?.Kill();
    }

    public void Show()
    {
        CurrentShow = true;

        foreach (var mr in meshRenderers)
        {
            mr.enabled = true;
        }

        foreach (var c in _colliders)
        {
            c.enabled = true;
        }

        _seq?.Kill();
        _seq = DOTween.Sequence();
        for (var i = 0; i < currentMaterials.Count; i++)
        {
            if (i == 0)
                _seq.Append(currentMaterials[i].DOFade(1.0f, _transitionDuration));
            else
                _seq.Join(currentMaterials[i].DOFade(1.0f, _transitionDuration));
        }

        _seq.OnComplete((() => { ExecuteTasksAfterShow(); }));
    }

    public void Hide(bool immediately = false, Action onComplete = null)
    {
        CurrentShow = false;

        _seq?.Kill();
        _seq = DOTween.Sequence();
        for (var i = 0; i < currentMaterials.Count; i++)
        {
            if (i == 0)
                _seq.Append(currentMaterials[i].DOFade(0.0f, immediately ? 0.0f : _transitionDuration));
            else
                _seq.Join(currentMaterials[i].DOFade(0.0f, immediately ? 0.0f : _transitionDuration));
        }

        // Callback
        _seq.OnComplete(() =>
        {
            foreach (var mr in meshRenderers)
            {
                mr.enabled = false;
            }

            foreach (var c in _colliders)
            {
                c.enabled = false;
            }

            onComplete?.Invoke();
        });
    }

    private void ExecuteTasksAfterShow()
    {
        foreach (var task in _tasksAfterShow)
        {
            task.Execute(this);
        }
    }

    protected override void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.TryGetComponent(out Laser laser) && laser.LaserSource == Laser.Source.Boss) return;
        base.OnCollisionEnter(other);
    }

    private void HandleInitHide()
    {
        foreach (var mr in meshRenderers)
        {
            mr.enabled = false;
        }

        foreach (var c in _colliders)
        {
            c.enabled = false;
        }

        foreach (var mat in currentMaterials)
        {
            Color color = mat.color;
            color.a = 0.0f;
            mat.color = color;
        }
    }
}