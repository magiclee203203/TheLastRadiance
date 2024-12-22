using Sirenix.OdinInspector;
using UnityEngine;

public class ChangeAppearanceCommand : ICommand
{
    [SerializeField, Required, AssetsOnly] private Material _newMaterial;

    public void Execute<T>(T receiver)
    {
        if (receiver is not IAppearanceChangeable obj) return;
        obj.CurrentMaterial = _newMaterial;
    }
}