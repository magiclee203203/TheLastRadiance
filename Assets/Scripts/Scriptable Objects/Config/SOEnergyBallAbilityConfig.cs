using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "SOEnergyBallAbilityConfig", menuName = "Scriptable Object/Configs/Energy Ball Ability")]
public class SOEnergyBallAbilityConfig : SOAbilityConfig
{
    [Required, BoxGroup("Prefabs"), LabelText("Energy Ball")]
    public GameObject EnergyBallPrefab;
}