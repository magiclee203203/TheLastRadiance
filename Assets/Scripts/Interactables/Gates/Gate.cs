using Animancer;
using Sirenix.OdinInspector;
using UnityEngine;

public class Gate : MonoBehaviour
{
    [SerializeField, Required] private int _lockNum;

    [SerializeField, AssetsOnly] [BoxGroup("Events Subscribed"), LabelText("Activate")]
    private SOOperateLockEvent _activateEvent;

    [SerializeField, AssetsOnly] [BoxGroup("Events Subscribed"), LabelText("Deactivate")]
    private SOOperateLockEvent _deactivateEvent;

    [SerializeField, Required, AssetsOnly] [BoxGroup("Animations")]
    private TransitionAsset _open;

    private AnimancerComponent _animancer;
    private AnimancerState _gateAnimationState;

    private bool _hasOpened;
    private bool _hasClosed;

    private void Awake()
    {
        _animancer = GetComponent<AnimancerComponent>();
    }

    private void OnEnable()
    {
        _activateEvent?.Subscribe(Activate);
        _deactivateEvent?.Subscribe(Deactivate);
    }

    private void OnDisable()
    {
        _activateEvent?.Unsubscribe(Activate);
        _deactivateEvent?.Unsubscribe(Deactivate);
    }

    private void Activate(int activeLockNum)
    {
        if (activeLockNum != _lockNum) return;

        _gateAnimationState = _animancer.Play(_open);

        _gateAnimationState.Speed = _open.Speed;
        _gateAnimationState.IsPlaying = true;
        _gateAnimationState.Events(this).OnEnd ??= () => { _gateAnimationState.IsPlaying = false; };

        // handle SFX
        _hasClosed = false;
        if (_hasOpened) return;
        AudioManager.Instance.Play(AudioManager.SoundType.DoorOperation);
        _hasOpened = true;
    }

    private void Deactivate(int deactiveLockNum)
    {
        if (deactiveLockNum != _lockNum) return;

        _gateAnimationState = _animancer.Play(_open);
        _gateAnimationState.IsPlaying = true;
        _gateAnimationState.Speed = _open.Speed * -1.0f;

        // handle SFX
        _hasOpened = false;
        if (_hasClosed) return;
        AudioManager.Instance.Play(AudioManager.SoundType.DoorOperation);
        _hasClosed = true;
    }
}