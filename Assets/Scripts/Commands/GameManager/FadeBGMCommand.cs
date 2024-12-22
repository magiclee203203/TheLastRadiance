using Sirenix.OdinInspector;
using UnityEngine;

public class FadeBGMCommand : ICommand
{
    private enum FadeType
    {
        FadeIn,
        FadeOut
    }

    [SerializeField, Required, EnumToggleButtons]
    private FadeType _fadeType;

    [SerializeField, Required] private AudioManager.SoundType _soundType;

    public void Execute<T>(T receiver)
    {
        switch (_fadeType)
        {
            case FadeType.FadeIn:
                AudioManager.Instance.FadeInBGM(_soundType);
                break;

            case FadeType.FadeOut:
                AudioManager.Instance.FadeOutBGM(_soundType);
                break;
        }
    }
}