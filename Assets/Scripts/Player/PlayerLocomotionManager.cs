using Animancer;
using Sirenix.OdinInspector;
using UnityEngine;

public class PlayerLocomotionManager : MonoBehaviour
{
    [SerializeField, Required, AssetsOnly] private TransitionAsset _animation;
    [SerializeField, Required, AssetsOnly] private StringAsset _mixerParameterName;
    [SerializeField, Required] private float _stateTransitionDuration;
    [SerializeField, Required] private float _rotateSpeed;

    private SmoothedFloatParameter _parameterValue;

    private void Start()
    {
        _parameterValue = new SmoothedFloatParameter(Player.Instance.Animancer, _mixerParameterName.Name,
            _stateTransitionDuration);
        Player.Instance.LocomotionLayer.Play(_animation);
    }

    private void Update()
    {
        UpdateSpeedParameter();
        Look();
    }

    private void OnAnimatorMove()
    {
        Move();
    }

    private void UpdateSpeedParameter()
    {
        if (Player.Instance.InputValue.MoveVector == Vector2.zero)
            _parameterValue.TargetValue = 0.0f;
        else
        {
            _parameterValue.TargetValue = Player.Instance.InputValue.IsPressingRun ? 1.0f : 0.5f;
            AudioManager.Instance.Play(Player.Instance.InputValue.IsPressingRun
                ? AudioManager.SoundType.PlayerRun
                : AudioManager.SoundType.PlayerWalk);
        }
    }

    private void Look()
    {
        if (Player.Instance.InputValue.MoveVector == Vector2.zero) return;

        var targetDir = Player.Instance.InputValue.MoveVector;
        var lookDir = Quaternion.LookRotation(new Vector3(targetDir.x, 0, targetDir.y).normalized.ToIsometric());
        var startRot = Player.Instance.Rb.rotation;
        var targetRot = Quaternion.Slerp(startRot, lookDir, _rotateSpeed * Time.deltaTime);
        Player.Instance.Rb.MoveRotation(targetRot);
    }

    private void Move()
    {
        var offset = Player.Instance.Animancer.Animator.deltaPosition;
        var currentPos = Player.Instance.Rb.position;
        Player.Instance.Rb.MovePosition(currentPos + offset);
    }
}