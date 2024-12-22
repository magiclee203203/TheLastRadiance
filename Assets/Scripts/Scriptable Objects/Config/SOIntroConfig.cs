using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "SOIntroConfig", menuName = "Scriptable Object/Configs/Intro")]
public class SOIntroConfig : ScriptableObject
{
    [SerializeField, Required, TextArea(4, 10)]
    public string Content;

    [SerializeField, Required] public Sprite Background;
}