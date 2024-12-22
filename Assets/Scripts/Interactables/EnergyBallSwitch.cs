using System.Collections.Generic;
using UnityEngine;

public class EnergyBallSwitch : EnergyBallReceiver, IAppearanceChangeable
{
    public Material CurrentMaterial
    {
        set
        {
            currentMaterials = new List<Material> { value };
            MaterialChanger.ChangeMaterial(value, ref meshRenderers);
        }
    }
}