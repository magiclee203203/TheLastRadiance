using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "SOOutroConfig", menuName = "Scriptable Object/Configs/Outro")]
public class SOOutroConfig : ScriptableObject
{
    [SerializeField, Required, TextArea(4, 10)]
    public string Content;
}