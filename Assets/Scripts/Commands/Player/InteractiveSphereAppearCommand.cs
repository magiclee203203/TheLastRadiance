using UnityEngine;

public class InteractiveSphereAppearCommand : ICommand
{
    public void Execute<T>(T receiver)
    {
        if (!GlobalVariablesManager.Instance.GetValue(VariableNamesDefine.PlayerLeftHand,
                out GameObject playerLeftHand))
        {
            Debug.LogError("Player Left Hand Not Found");
            return;
        }

        Object.Instantiate(GameStateManager.Instance.ActiveAbilityConfig.InteractiveSpherePrefab,
            playerLeftHand.transform.position, Quaternion.identity);
    }
}