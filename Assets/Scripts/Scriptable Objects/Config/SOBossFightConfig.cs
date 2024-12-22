using System.Collections.Generic;
using Animancer;
using BehaviorDesigner.Runtime;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "SOBossFightConfig", menuName = "Scriptable Object/Configs/Boss Fight")]
public class SOBossFightConfig : ScriptableObject
{
    [SerializeField, Required, AssetsOnly] public ExternalBehaviorTree BossFightAI;

    [SerializeField, Required, AssetsOnly] [BoxGroup("Boss Animations"), LabelText("Idle")]
    public TransitionAsset IdleStateAnimation;

    [SerializeField, Required, AssetsOnly] [BoxGroup("Boss Animations"), LabelText("Aim")]
    public TransitionAsset AimAnimation;

    [SerializeField, Required, AssetsOnly] [BoxGroup("Boss Animations"), LabelText("Shoot")]
    public TransitionAsset ShootAnimation;

    [SerializeField, Required, AssetsOnly] [BoxGroup("Boss Animations"), LabelText("Be Hit")]
    public TransitionAsset BeHitAnimation;

    [SerializeField, Required, AssetsOnly] [BoxGroup("Boss Animations"), LabelText("Stand Up")]
    public TransitionAsset StandUpAnimation;

    [SerializeField, Required, AssetsOnly] [BoxGroup("Boss Animations"), LabelText("Defeated")]
    public TransitionAsset DefeatedAnimation;

    [SerializeField, Required, AssetsOnly] public List<GameObject> TurretsRandomPool;
}