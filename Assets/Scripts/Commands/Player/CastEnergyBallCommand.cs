using UnityEngine;

public class CastEnergyBallCommand : ICommand
{
    public void Execute<T>(T receiver)
    {
        if (!GlobalVariablesManager.Instance.GetValue(VariableNamesDefine.PlayerLeftHand,
                out GameObject playerLeftHand))
        {
            Debug.LogError("Player Left Hand Not Found");
            return;
        }

        var cfg = GameStateManager.Instance.ActiveAbilityConfig as SOEnergyBallAbilityConfig;

        // calculate energy ball rotation
        var forwardDir = -playerLeftHand.transform.right;
        forwardDir.y = 0.0f;
        forwardDir.Normalize();
        var targetRot = Quaternion.LookRotation(forwardDir);

        Object.Instantiate(cfg!.EnergyBallPrefab, playerLeftHand.transform.position, targetRot);

        AudioManager.Instance.Play(AudioManager.SoundType.ShootEnergyBall);
    }
}