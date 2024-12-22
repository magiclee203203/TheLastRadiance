using UnityEngine;

[CreateAssetMenu(fileName = "Sound", menuName = "Scriptable Object/SFX/Sound")]
public class SOSound : ScriptableObject
{
    public AudioManager.SoundType SoundType;
    public AudioClip Clip;
    [Range(0, 1)] public float Volume = 1.0f;
    public bool Loop;
    public float DelayTime;
}