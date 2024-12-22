using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[ShowOdinSerializedPropertiesInInspector]
public class RespawnManager : Singleton<RespawnManager>
{
    [SerializeField, Required, AssetsOnly] [BoxGroup("Events Subscribed"), LabelText("Player Death")]
    private SOPlayerDeathEvent _playerDeathEvent;

    [SerializeField, Required, AssetsOnly] [BoxGroup("Events Subscribed"), LabelText("Death Fade Out End")]
    private SOEvent _playerDeathFadeOutEndEvent;

    [SerializeField, Required, AssetsOnly] [BoxGroup("Events Subscribed"), LabelText("Player Defeated By Boss")]
    private SOEvent _playerDefeatedByBossEvent;

    [SerializeField, Required] private Dictionary<string, GameObject> _respawnPrefabDict = new();
    [SerializeField, Required] private Dictionary<string, Transform> _playerRespawnPointDict = new();
    [SerializeField, Required] private Dictionary<string, Transform> _roomRespawnTransformDict = new();

    private void OnEnable()
    {
        _playerDeathEvent.Subscribe(HandleRespawn);
        _playerDeathFadeOutEndEvent.Subscribe(OnPlayerDeathFadeOutEnd);
        _playerDefeatedByBossEvent.Subscribe(OnPlayerDefeatedByBoss);
    }

    private void OnDisable()
    {
        _playerDeathEvent.Unsubscribe(HandleRespawn);
        _playerDeathFadeOutEndEvent.Unsubscribe(OnPlayerDeathFadeOutEnd);
        _playerDefeatedByBossEvent.Unsubscribe(OnPlayerDefeatedByBoss);
    }

    private void HandleRespawn(DeathTrigger trigger)
    {
        // read Transform Info
        var roomName = trigger.RoomName;

        if (!_respawnPrefabDict.TryGetValue(roomName, out var newRoomPrefab))
        {
            Debug.LogError($"{roomName} not found");
            return;
        }

        if (!_playerRespawnPointDict.TryGetValue(roomName, out var playerRespawnPosition))
        {
            Debug.LogError("Player respawn point not found");
            return;
        }

        if (!_roomRespawnTransformDict.TryGetValue(roomName, out var roomRespawnTransform))
        {
            Debug.LogError("Player respawn point not found");
            return;
        }

        // respawn
        Destroy(trigger.gameObject);

        var newRoom = Instantiate(newRoomPrefab, roomRespawnTransform.position, roomRespawnTransform.rotation);
        newRoom.name = roomName;

        Player.Instance.Rb.interpolation = RigidbodyInterpolation.None;
        Player.Instance.Rb.linearVelocity = Vector3.zero;
        Player.Instance.transform.position = playerRespawnPosition.position;

        // handle Ability Sphere. This can happen in Room 7, where player can hold the crate and jump to death
        if (GlobalVariablesManager.Instance.GetValue(VariableNamesDefine.AbilitySphere, out GameObject abilitySphere))
            Destroy(abilitySphere);
    }

    private void OnPlayerDeathFadeOutEnd()
    {
        Player.Instance.Rb.interpolation = RigidbodyInterpolation.Interpolate;

        GlobalVariablesManager.Instance.RemoveValue(VariableNamesDefine.TargetEnergyBallReceiver);
        GlobalVariablesManager.Instance.RemoveValue(VariableNamesDefine.TargetGravityObject);
    }

    private void OnPlayerDefeatedByBoss()
    {
        StartCoroutine(DelayRespawningPlayerForBossFight());
    }

    private IEnumerator DelayRespawningPlayerForBossFight()
    {
        yield return new WaitForSeconds(1f);

        if (!_playerRespawnPointDict.TryGetValue(VariableNamesDefine.BossFightRespawnPointKey,
                out Transform playerRespawnPosition))
        {
            Debug.LogError("Player respawn point not found");
            yield return null;
        }

        Player.Instance.Rb.linearVelocity = Vector3.zero;
        Player.Instance.transform.position = playerRespawnPosition.position;
    }
}