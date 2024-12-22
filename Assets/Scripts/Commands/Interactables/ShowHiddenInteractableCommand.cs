using Sirenix.OdinInspector;
using UnityEngine;

public class ShowHiddenInteractableCommand : ICommand
{
    [SerializeField, Required, SceneObjectsOnly]
    private IHidden _hiddenInteractable;

    public void Execute<T>(T receiver)
    {
        if (!_hiddenInteractable.CurrentShow)
            _hiddenInteractable.Show();
    }
}