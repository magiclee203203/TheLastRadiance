using Sirenix.OdinInspector;
using UnityEngine;

public class PlayerDefeatedByBoss : MonoBehaviour
{
    [SerializeField, Required, AssetsOnly] [BoxGroup("Events Published"), LabelText("Player Defeated By Boss")]
    private SOEvent _playerDefeatedByBossEvent;

    private void OnCollisionEnter(Collision other)
    {
        if (!other.gameObject.TryGetComponent(out Laser laser)) return;
        if (laser.LaserSource != Laser.Source.Boss) return;

        _playerDefeatedByBossEvent.Notify();
    }
}