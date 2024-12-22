using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class GameStateManager : Singleton<GameStateManager>
{
    [SerializeField, Required, AssetsOnly] [BoxGroup("Events Subscribed"), LabelText("Change Active Ability")]
    private SOEvent _changeActiveAbilityEvent;

    [SerializeField, Required, AssetsOnly] [BoxGroup("Events Published"), LabelText("Update Ability Panel")]
    private SOShowAbilityIndicatorEvent _updateAbilityPanelEvent;

    [SerializeField] private List<SOAbilityConfig> _availableAbilities = new();
    private int _activeAbilityIndex;

    public SOAbilityConfig ActiveAbilityConfig =>
        _availableAbilities.Count == 0 ? null : _availableAbilities[_activeAbilityIndex];

    private bool _isNPCActive;
    public bool IsNPCActive => _isNPCActive;

    private void OnEnable()
    {
        _changeActiveAbilityEvent.Subscribe(OnChangeActiveAbility);
    }

    private void OnDisable()
    {
        _changeActiveAbilityEvent.Unsubscribe(OnChangeActiveAbility);
    }

    private void OnChangeActiveAbility()
    {
        _activeAbilityIndex += 1;
        if (_activeAbilityIndex >= _availableAbilities.Count)
            _activeAbilityIndex = 0;

        if (ActiveAbilityConfig == null) return;
        _updateAbilityPanelEvent.Notify(ActiveAbilityConfig.Name);
        AudioManager.Instance.Play(AudioManager.SoundType.SwitchAbility);
    }

    public void EnableNPC()
    {
        _isNPCActive = true;
    }

    public void StudyNewAbility(SOAbilityConfig abilityConfig)
    {
        _availableAbilities.Add(abilityConfig);
    }
}