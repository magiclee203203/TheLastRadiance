using System.Collections.Generic;
using Animancer;
using Sirenix.OdinInspector;
using UnityEngine;

public abstract class SOAbilityConfig : SerializedScriptableObject
{
    [Required] public string Name;

    [Required, BoxGroup("Tasks"), TitleGroup("Tasks/Charge Step"), LabelText("On Enable")]
    public readonly List<ICommand> ChargeStepOnEnableTasks;

    [Required, BoxGroup("Tasks"), TitleGroup("Tasks/Charge Step"), LabelText("On Disable")]
    public readonly List<ICommand> ChargeStepOnDisableTasks;

    [Required, BoxGroup("Tasks"), TitleGroup("Tasks/Cast Step"), LabelText("On Enable")]
    public readonly List<ICommand> CastStepOnEnableTasks;

    [Required, BoxGroup("Tasks"), TitleGroup("Tasks/Cast Step"), LabelText("On Disable")]
    public readonly List<ICommand> CastStepOnDisableTasks;

    [Required, BoxGroup("Animations"), LabelText("Charge")]
    public TransitionAsset ChargeAnimation;

    [Required, BoxGroup("Animations"), LabelText("Cast")]
    public TransitionAsset CastAnimation;

    [SerializeField, Required, AssetsOnly] [BoxGroup("Events Published"), LabelText("Charge Animation End")]
    public SOEvent ChargeAnimationEndEvent;

    [SerializeField, Required, AssetsOnly] [BoxGroup("Events Published"), LabelText("Cast Midway")]
    public SOEvent CastMidwayEvent;

    [SerializeField, Required, AssetsOnly] [BoxGroup("Events Published"), LabelText("Cast Animation End")]
    public SOEvent CastAnimationEndEvent;

    [Required, BoxGroup("Prefabs"), LabelText("Interactive Sphere")]
    public GameObject InteractiveSpherePrefab;
}