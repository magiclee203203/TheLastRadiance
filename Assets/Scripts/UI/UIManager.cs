using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;

[ShowOdinSerializedPropertiesInInspector]
public class UIManager : Singleton<UIManager>
{
    [SerializeField, Required, SceneObjectsOnly]
    private PlayerDeathPanel _playerDeathPanel;

    [SerializeField, Required, SceneObjectsOnly]
    private PopupPanel _popupPanel;

    [SerializeField, Required, SceneObjectsOnly]
    private AbilityPanel _abilityPanel;

    [SerializeField, Required, SceneObjectsOnly]
    private PausePanel _pausePanel;

    [SerializeField, SceneObjectsOnly] private StartPanel _startPanel;
    [SerializeField, SceneObjectsOnly] private IntroPanel _introPanel;

    [SerializeField, Required, SceneObjectsOnly]
    private OutroPanel _outroPanel;

    [SerializeField, Required, AssetsOnly]
    [BoxGroup("Events Published"), LabelText("Death Fade Out End")]
    private SOEvent _playerDeathFadeOutEndEvent;

    [SerializeField, Required, AssetsOnly]
    [BoxGroup("Events Subscribe"), LabelText("Show Popup")]
    private SOShowPopupEvent _showPopupEvent;

    [SerializeField, Required, AssetsOnly]
    [BoxGroup("Events Subscribed"), LabelText("Show Ability")]
    private SOShowAbilityIndicatorEvent _showAbilityEvent;

    [SerializeField, Required, AssetsOnly]
    [BoxGroup("Events Subscribed"), LabelText("Conversation Start")]
    private SOEvent _genericConversationStartEvent;

    [SerializeField, Required, AssetsOnly]
    [BoxGroup("Events Subscribed"), LabelText("Conversation End")]
    private SOEvent _genericConversationEndEvent;

    [SerializeField, Required, AssetsOnly]
    [BoxGroup("Events Subscribed"), LabelText("Player Press Pause")]
    private SOEvent _playerPressPauseEvent;

    [SerializeField, Required, AssetsOnly]
    [BoxGroup("Events Subscribed"), LabelText("Show Outro Panel")]
    private SOEvent _showOutroPanelEvent;

    [SerializeField, Required, AssetsOnly]
    [BoxGroup("Events Subscribed"), LabelText("Player Death")]
    private SOPlayerDeathEvent _playerDeathEvent;

    [SerializeField, Required, AssetsOnly]
    [BoxGroup("Events Published"), LabelText("Player Defeated By Boss")]
    private SOEvent _playerDefeatedByBossEvent;

    private PlayerInputActions _inputActions;
    private IPanel _activePanel;

    private void Awake()
    {
        _inputActions = new PlayerInputActions();
        _inputActions.Disable();

        InitPanelsState();
        SetCallbacks();
    }

    private void OnEnable()
    {
        SubscribeInputActions();

        _showPopupEvent.Subscribe(OnShowPopup);
        _showAbilityEvent.Subscribe(OnShowAbilityPanel);
        _genericConversationStartEvent.Subscribe(OnHideAbilityPanel);
        _genericConversationEndEvent.Subscribe(OnReShowAbilityPanel);
        _playerPressPauseEvent.Subscribe(OnShowPauseMenu);
        _showOutroPanelEvent.Subscribe(ShowOutroPanel);
        _playerDeathEvent.Subscribe(OnPlayerDeath);
        _playerDefeatedByBossEvent.Subscribe(OnPlayerDefeatedByBoss);
    }

    private void OnDisable()
    {
        UnsubscribeInputActions();

        _showPopupEvent.Unsubscribe(OnShowPopup);
        _showAbilityEvent.Unsubscribe(OnShowAbilityPanel);
        _genericConversationStartEvent.Unsubscribe(OnHideAbilityPanel);
        _genericConversationEndEvent.Unsubscribe(OnReShowAbilityPanel);
        _playerPressPauseEvent.Unsubscribe(OnShowPauseMenu);
        _showOutroPanelEvent.Unsubscribe(ShowOutroPanel);
        _playerDeathEvent.Unsubscribe(OnPlayerDeath);
        _playerDefeatedByBossEvent.Unsubscribe(OnPlayerDefeatedByBoss);
    }

    private void Start()
    {
        ShowStartPanel();
    }

    private void OnPressContinue(InputAction.CallbackContext ctx)
    {
        _activePanel.Continue();
    }

    private void OnShowPopup(SOPopupPanelConfig config)
    {
        _popupPanel.Config = config;
        _popupPanel.Show();
        _activePanel = _popupPanel;

        EnableUIInputAndDisablePlayerInput(true);
        AudioManager.Instance.Play(config.SFX.SoundType);
    }

    private void OnShowAbilityPanel(string abilityName)
    {
        _abilityPanel.ShowAbility(abilityName);
    }

    private void OnHideAbilityPanel()
    {
        _abilityPanel.HideAbility();
    }

    private void OnReShowAbilityPanel()
    {
        _abilityPanel.ReShow();
    }

    private void OnShowPauseMenu()
    {
        _pausePanel.Show();
        _activePanel = _pausePanel;

        Time.timeScale = 0.0f;
        EnableUIInputAndDisablePlayerInput(true);
        AudioManager.Instance.Play(AudioManager.SoundType.ShowPauseMenu);
    }

    private void OnSelectPauseButton(InputAction.CallbackContext ctx)
    {
        if (_activePanel is not PausePanel) return;

        var inputVal = ctx.ReadValue<Vector2>();
        var idx = inputVal.y switch
        {
            > 0 => -1,
            < 0 => 1,
            _ => 0
        };
        _pausePanel.SelectButton(idx);
    }

    private void OnPlayerDeath(DeathTrigger _)
    {
        _playerDeathPanel.Show();
        EnableUIInputAndDisablePlayerInput(true);
    }

    private void OnPlayerDefeatedByBoss()
    {
        _playerDeathPanel.Show();
        EnableUIInputAndDisablePlayerInput(true);
    }

    private void SubscribeInputActions()
    {
        _inputActions.UI.Continue.started += OnPressContinue;
        _inputActions.UI.SelectMenuButton.started += OnSelectPauseButton;
    }

    private void UnsubscribeInputActions()
    {
        _inputActions.UI.Continue.started -= OnPressContinue;
        _inputActions.UI.SelectMenuButton.started -= OnSelectPauseButton;
    }

    private void ShowStartPanel()
    {
        _startPanel.Show();
        _activePanel = _startPanel;
        EnableUIInputAndDisablePlayerInput(true);
    }

    public void ShowIntroPanel()
    {
        _introPanel.Show();
        _activePanel = _introPanel;
        EnableUIInputAndDisablePlayerInput(true);
    }

    private void ShowOutroPanel()
    {
        _outroPanel.Show();
        _activePanel = _outroPanel;
        EnableUIInputAndDisablePlayerInput(true);
    }

    private void InitPanelsState()
    {
        _startPanel.gameObject.SetActive(true);
        _abilityPanel.gameObject.SetActive(true);

        _introPanel.gameObject.SetActive(false);
        _pausePanel.gameObject.SetActive(false);
        _popupPanel.gameObject.SetActive(false);
        _outroPanel.gameObject.SetActive(false);
        _playerDeathPanel.gameObject.SetActive(false);
    }

    private void SetCallbacks()
    {
        HandlePopupPanelCallback();
        HandlePausePanelCallback();
        HandleStartPanelCallback();
        HandleIntroPanelCallback();
        HandleOutroPanelCallback();
        HandlePlayerDeathPanelCallback();
    }

    private void HandlePopupPanelCallback()
    {
        _popupPanel.OnClose = tasks =>
        {
            EnableUIInputAndDisablePlayerInput(false);

            foreach (var task in tasks)
            {
                task.Execute(this);
            }
        };
    }

    private void HandlePausePanelCallback()
    {
        _pausePanel.OnConfirm = buttonType =>
        {
            switch (buttonType)
            {
                case PausePanel.ButtonType.Continue:
                    _pausePanel.Hide();
                    Time.timeScale = 1.0f;
                    break;

                case PausePanel.ButtonType.QuitGame:
                    Application.Quit();
                    break;
            }

            EnableUIInputAndDisablePlayerInput(false);
        };
    }

    private void HandleStartPanelCallback()
    {
        _startPanel.OnEnd = tasks =>
        {
            EnableUIInputAndDisablePlayerInput(false);

            foreach (var task in tasks)
            {
                task.Execute(this);
            }
        };
    }

    private void HandleIntroPanelCallback()
    {
        _introPanel.OnEnd = tasks =>
        {
            EnableUIInputAndDisablePlayerInput(false);

            foreach (var task in tasks)
            {
                task.Execute(this);
            }
        };
    }

    private void HandleOutroPanelCallback()
    {
        _outroPanel.OnEnd = Application.Quit;
    }

    private void HandlePlayerDeathPanelCallback()
    {
        _playerDeathPanel.OnEnd = () =>
        {
            EnableUIInputAndDisablePlayerInput(false);
            _playerDeathFadeOutEndEvent.Notify();
        };
    }

    private void EnableUIInputAndDisablePlayerInput(bool yes)
    {
        if (yes)
            _inputActions.Enable();
        else
            _inputActions.Disable();

        Player.Instance.InputManager.SetEnableState(!yes);
    }
}