using System.Collections.Generic;
using PixelCrushers.DialogueSystem;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MyDialoguesManager : SerializedMonoBehaviour
{
    [SerializeField, Required, AssetsOnly] [BoxGroup("Events Subscribed"), LabelText("Trigger Dialogue")]
    private SODialogueTriggerEvent _dialogueTriggerEvent;

    [SerializeField, Required, AssetsOnly] [BoxGroup("Events Published"), LabelText("Conversation Start")]
    private SOEvent _genericConversationStartEvent;

    [SerializeField, Required, AssetsOnly] [BoxGroup("Events Published"), LabelText("Conversation End")]
    private SOEvent _genericConversationEndEvent;

    [SerializeField, Required, SceneObjectsOnly] [BoxGroup("UI Buttons"), LabelText("Player Continue")]
    private Button _playerContinueBtn;

    [SerializeField, Required, SceneObjectsOnly] [BoxGroup("UI Buttons"), LabelText("NPC Continue")]
    private Button _npcContinueBtn;

    [SerializeField, Required] private Dictionary<string, List<ICommand>> _tasksAfterConversation;
    [SerializeField, Required] private List<string> _forbiddenGenericConversationEnd = new();

    private PlayerInputActions _inputActions;
    private readonly List<string> _activatedConversationTitles = new();
    private string _currentConversationTitle;

    enum Speaker
    {
        Player,
        NPC
    }

    private Speaker _currentSpeaker = Speaker.Player;

    private void Awake()
    {
        _inputActions = new PlayerInputActions();
        _inputActions.Disable();
    }

    private void OnEnable()
    {
        DialogueManager.instance.conversationStarted += OnConversationStart;
        DialogueManager.instance.conversationEnded += OnConversationEnd;

        _inputActions.Dialogue.Continue.started += OnPressContinueConversation;

        _dialogueTriggerEvent.Subscribe(OnDialogueTrigger);
    }

    private void OnDisable()
    {
        DialogueManager.instance.conversationStarted -= OnConversationStart;
        DialogueManager.instance.conversationEnded -= OnConversationEnd;

        _inputActions.Dialogue.Continue.started -= OnPressContinueConversation;

        _dialogueTriggerEvent.Unsubscribe(OnDialogueTrigger);
    }

    private void OnConversationStart(Transform t)
    {
        Player.Instance.InputManager.SetEnableState(false);
        _inputActions.Enable();

        // Notify
        _genericConversationStartEvent.Notify();
    }

    private void OnConversationEnd(Transform t)
    {
        Player.Instance.InputManager.SetEnableState(true);
        _inputActions.Disable();

        // Do something
        if (_tasksAfterConversation.TryGetValue(_currentConversationTitle, out var tasks))
        {
            foreach (var task in tasks)
            {
                task.Execute(this);
            }
        }

        // Notify
        if (!_forbiddenGenericConversationEnd.Contains(_currentConversationTitle))
            _genericConversationEndEvent.Notify();

        _currentConversationTitle = "";
    }

    private void OnPressContinueConversation(InputAction.CallbackContext _)
    {
        switch (_currentSpeaker)
        {
            case Speaker.Player:
                _playerContinueBtn.onClick.Invoke();
                break;
            case Speaker.NPC:
                _npcContinueBtn.onClick.Invoke();
                break;
        }
    }

    private void OnDialogueTrigger(string conversationTitle)
    {
        if (_activatedConversationTitles.Contains(conversationTitle)) return;

        _activatedConversationTitles.Add(conversationTitle);
        DialogueManager.StartConversation(conversationTitle);

        _currentConversationTitle = conversationTitle;
    }

    public void OnPlayerSubtitlePanelFocus()
    {
        _currentSpeaker = Speaker.Player;
    }

    public void OnNPCSubtitlePanelFocus()
    {
        _currentSpeaker = Speaker.NPC;
    }
}