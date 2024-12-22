using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

[ShowOdinSerializedPropertiesInInspector]
public class AudioManager : Singleton<AudioManager>
{
    public enum SoundType
    {
        PlayerWalk,
        PlayerRun,
        ShowAbilitySphere,
        ShootEnergyBall,
        EnergyBallExplosion,
        DoorOperation,
        GetGloveAndAbility,
        GeneralPopup,
        ShowPauseMenu,
        MovePauseMenuButton,
        ConfirmInPauseMenu,
        HiddenBridgeShow,
        TurretCharge,
        BossAim,
        BossShoot,
        BossStandUp,
        BossArmFlash,
        BossDefeated,
        SwitchAbility,
        BGM,
        BossFightBGM
    }

    [SerializeField, Required] private Dictionary<SoundType, SOSound> _soundDict = new();
    [SerializeField, Required] private float _fadeBGMDuration = 0.8f;
    private Dictionary<SoundType, AudioSource> _sourceDict = new();
    private Dictionary<SoundType, float> _lastPlayTimeDict = new();

    private void Awake()
    {
        foreach (var kv in _soundDict)
        {
            var source = gameObject.AddComponent<AudioSource>();
            source.clip = kv.Value.Clip;
            source.volume = kv.Value.Volume;
            source.loop = kv.Value.Loop;
            source.playOnAwake = false;

            _sourceDict.Add(kv.Key, source);
        }
    }

    private void Start()
    {
        Play(SoundType.BGM);
    }

    public void Play(SoundType soundType)
    {
        if (!_sourceDict.TryGetValue(soundType, out var source) || !_soundDict.TryGetValue(soundType, out var sound))
        {
            Debug.LogError($"{soundType} not found");
            return;
        }

        if (!_lastPlayTimeDict.TryGetValue(sound.SoundType, out var lastPlayTime))
        {
            // play the first time
            source.Play();
            _lastPlayTimeDict[sound.SoundType] = Time.time;
            return;
        }

        // handle delay
        if (!(Time.time - lastPlayTime >= sound.DelayTime)) return;
        source.Play();
        _lastPlayTimeDict[sound.SoundType] = Time.time;
    }

    public void Stop(SoundType soundType)
    {
        if (!_sourceDict.TryGetValue(soundType, out var source))
        {
            Debug.LogError($"{soundType} not found");
            return;
        }

        source.Stop();
    }

    public void FadeOutBGM(SoundType soundType)
    {
        if (!_sourceDict.TryGetValue(soundType, out var source))
        {
            Debug.LogError($"{soundType} not found");
            return;
        }

        source.DOFade(0.0f, _fadeBGMDuration).OnComplete(() => { source.Stop(); });
    }

    public void FadeInBGM(SoundType soundType)
    {
        if (!_sourceDict.TryGetValue(soundType, out var source) || !_soundDict.TryGetValue(soundType, out var sound))
        {
            Debug.LogError($"{soundType} not found");
            return;
        }

        source.volume = 0.0f;
        source.Play();
        source.DOFade(sound.Volume, _fadeBGMDuration);
    }
}