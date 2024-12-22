using Sirenix.OdinInspector;
using UnityEngine;

public class StudyAbilityCommand : ICommand
{
    [SerializeField, Required, AssetsOnly] private SOAbilityConfig _abilityConfig;

    public void Execute<T>(T receiver)
    {
        GameStateManager.Instance.StudyNewAbility(_abilityConfig);
    }
}