using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class InteractableFlashVFX : MonoBehaviour
{
    [SerializeField, AssetsOnly] private SOInteractableFlashVFXConfig _energyBallReceiverCfg;
    [SerializeField, AssetsOnly] private SOInteractableFlashVFXConfig _gravityObjectCfg;
    [SerializeField, AssetsOnly] private SOInteractableFlashVFXConfig _pressedPlateCfg;

    private int _activeGlowMaterialIndex;
    private Coroutine _flashCoroutine;

    public void StartFlash(InteractableType interactableType, List<MeshRenderer> meshRenderers)
    {
        if (_flashCoroutine != null) return;

        var cfg = GetConfig(interactableType);
        _flashCoroutine = StartCoroutine(FlashCoroutine(cfg, meshRenderers));
    }

    public void StopFlash(List<MeshRenderer> meshRenderers, List<Material> defaultMaterials)
    {
        if (_flashCoroutine == null) return;
        StopCoroutine(_flashCoroutine);
        _flashCoroutine = null;

        // reset material
        ResetMaterials(ref meshRenderers, defaultMaterials);
        _activeGlowMaterialIndex = 0;
    }

    private IEnumerator FlashCoroutine(SOInteractableFlashVFXConfig cfg, List<MeshRenderer> meshRenderers)
    {
        while (true)
        {
            var newMat = cfg.GlowMaterials[_activeGlowMaterialIndex];
            MaterialChanger.ChangeMaterial(newMat, ref meshRenderers);

            _activeGlowMaterialIndex += 1;
            if (_activeGlowMaterialIndex >= cfg.GlowMaterials.Count)
                _activeGlowMaterialIndex = 0;

            yield return new WaitForSeconds(cfg.TransitionDuration);
        }
    }

    private void ResetMaterials(ref List<MeshRenderer> meshRenderers, List<Material> defaultMaterials)
    {
        for (var i = 0; i < meshRenderers.Count; i++)
        {
            var defaultMaterial = new[] { defaultMaterials[i] };
            meshRenderers[i].materials = defaultMaterial;
        }
    }

    private SOInteractableFlashVFXConfig GetConfig(InteractableType interactableType)
    {
        return interactableType switch
        {
            InteractableType.EnergyBallReceiver => _energyBallReceiverCfg,
            InteractableType.GravityObject => _gravityObjectCfg,
            InteractableType.PressedPlate => _pressedPlateCfg,
            _ => null
        };
    }
}