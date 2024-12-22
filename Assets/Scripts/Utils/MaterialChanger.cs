using System.Collections.Generic;
using UnityEngine;

public static class MaterialChanger
{
    public static void ChangeMaterial(Material mat, ref List<MeshRenderer> meshRenderers)
    {
        var newMats = new List<Material> { mat };
        foreach (var mr in meshRenderers)
        {
            mr.SetMaterials(newMats);
        }
    }

    public static void FindMeshRenderers(Transform parent, out List<MeshRenderer> meshRenderers,
        out List<Material> currentMaterials)
    {
        meshRenderers = new List<MeshRenderer>();
        currentMaterials = new List<Material>();

        parent.GetComponentsInChildren(false, meshRenderers);
        foreach (var meshRenderer in meshRenderers)
        {
            currentMaterials.Add(meshRenderer.materials[0]);
        }
    }
}