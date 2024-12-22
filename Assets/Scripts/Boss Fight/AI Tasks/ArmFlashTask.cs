using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

[TaskCategory("Custom"), TaskName("Boss Arm Flash")]
public class ArmFlashTask : Action
{
    [SerializeField] private List<Material> _flashMaterials;
    [SerializeField] private float _flashInterval = 0.4f;

    private GameObject _flashedArm;
    private GameObject _flashedHand;

    private List<SkinnedMeshRenderer> _armRenderers;
    private List<SkinnedMeshRenderer> _handRenderers;

    private List<Material> _armMaterials;
    private List<Material> _handMaterials;

    private int _currentFlashMatIndex;
    private Coroutine _flashCoroutine;

    public override void OnAwake()
    {
        if (!GlobalVariablesManager.Instance.GetValue(VariableNamesDefine.BossFlashedArm, out _flashedArm))
            Debug.LogError("Flashed Arm not found");

        if (!GlobalVariablesManager.Instance.GetValue(VariableNamesDefine.BossFlashedHand, out _flashedHand))
            Debug.LogError("Flashed Hand not found");

        FindMeshRenderers(_flashedArm.transform, out _armRenderers, out _armMaterials);
        FindMeshRenderers(_flashedHand.transform, out _handRenderers, out _handMaterials);
    }

    public override void OnStart()
    {
        StartFlash();
        AudioManager.Instance.Play(AudioManager.SoundType.BossArmFlash);
    }

    public override void OnEnd()
    {
        // Stop Flashing
        StopFlash();

        // Reset Material
        ChangeMaterial(_armMaterials[0], ref _armRenderers);
        ChangeMaterial(_handMaterials[0], ref _handRenderers);

        AudioManager.Instance.Stop(AudioManager.SoundType.BossArmFlash);
    }

    public override TaskStatus OnUpdate()
    {
        return TaskStatus.Running;
    }

    private void StartFlash()
    {
        if (_flashCoroutine != null)
        {
            Owner.StopCoroutine(_flashCoroutine);
            _flashCoroutine = null;
        }

        _flashCoroutine = Owner.StartCoroutine(FlashCoroutine());
    }

    private void StopFlash()
    {
        Owner.StopCoroutine(_flashCoroutine);
        _flashCoroutine = null;
    }

    private IEnumerator FlashCoroutine()
    {
        while (true)
        {
            ChangeMaterial(_flashMaterials[_currentFlashMatIndex], ref _armRenderers);
            ChangeMaterial(_flashMaterials[_currentFlashMatIndex], ref _handRenderers);

            yield return new WaitForSeconds(_flashInterval);
            _currentFlashMatIndex += 1;
            if (_currentFlashMatIndex >= _flashMaterials.Count)
                _currentFlashMatIndex = 0;

            ChangeMaterial(_flashMaterials[_currentFlashMatIndex], ref _armRenderers);
            ChangeMaterial(_flashMaterials[_currentFlashMatIndex], ref _handRenderers);
        }
    }

    private void FindMeshRenderers(Transform parent, out List<SkinnedMeshRenderer> meshRenderers,
        out List<Material> currentMaterials)
    {
        meshRenderers = new List<SkinnedMeshRenderer>();
        currentMaterials = new List<Material>();

        parent.GetComponentsInChildren(false, meshRenderers);
        foreach (var meshRenderer in meshRenderers)
        {
            currentMaterials.Add(meshRenderer.materials[0]);
        }
    }

    private void ChangeMaterial(Material mat, ref List<SkinnedMeshRenderer> meshRenderers)
    {
        var newMats = new List<Material> { mat };
        foreach (var mr in meshRenderers)
        {
            mr.SetMaterials(newMats);
        }
    }
}