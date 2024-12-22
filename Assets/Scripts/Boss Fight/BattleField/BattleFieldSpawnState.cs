using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

public class BattleFieldSpawnState : BattleFieldBaseState
{
    public Action StateEndCallback;
    private EnergyLaserTurret _turret;

    public BattleFieldSpawnState(BattleField context) : base(context)
    {
    }

    public override void OnEnterState()
    {
        var turretObj = GenerateATurret();
        turretObj.transform.parent = context.transform;
        _turret = turretObj.GetComponent<EnergyLaserTurret>();
        _turret.Show();
    }

    private GameObject GenerateATurret()
    {
        var turretPrefab = PickARandomElement(context.BossFightCfg.TurretsRandomPool);
        var spawnPoint = PickARandomElement(context.SpawnPoints);
        return Object.Instantiate(turretPrefab, spawnPoint.position, turretPrefab.transform.rotation);
    }

    private T PickARandomElement<T>(List<T> items)
    {
        var index = Random.Range(0, items.Count);
        return items[index];
    }

    public void TurretActivate()
    {
        // in order to deactivate pressed plate
        _turret.transform.DOMoveY(1.0f, 0.8f).SetRelative().OnComplete(HideTurret);
    }

    private void HideTurret()
    {
        _turret.Hide(false, () =>
        {
            Object.Destroy(_turret.gameObject);
            StateEndCallback?.Invoke();
        });
    }
}