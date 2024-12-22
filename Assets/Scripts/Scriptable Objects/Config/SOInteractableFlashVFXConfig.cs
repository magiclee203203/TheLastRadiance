using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "SOInteractableFlashVFXConfig",
    menuName = "Scriptable Object/Configs/Interactable Flash VFX")]
public class SOInteractableFlashVFXConfig : ScriptableObject
{
    [Required, AssetsOnly] public List<Material> GlowMaterials;
    [Required] public float TransitionDuration = 1.0f;
}