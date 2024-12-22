using Animancer;
using Sirenix.OdinInspector;
using UnityEngine;

public class StoryGate : MonoBehaviour
{
    [SerializeField, Required, AssetsOnly] private TransitionAsset _openAnimation;
    private AnimancerComponent _animancer;

    private bool _hasOpened;

    private void Awake()
    {
        _animancer = GetComponent<AnimancerComponent>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(VariableNamesDefine.PlayerTag) || !GameStateManager.Instance.IsNPCActive) return;
        _animancer.Play(_openAnimation);

        if (_hasOpened) return;
        AudioManager.Instance.Play(AudioManager.SoundType.DoorOperation);
        _hasOpened = true;
    }
}