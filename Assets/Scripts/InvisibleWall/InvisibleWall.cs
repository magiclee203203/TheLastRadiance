using UnityEngine;

public class InvisibleWall : MonoBehaviour
{
    private Material _mat;

    private void Awake()
    {
        _mat = GetComponent<MeshRenderer>().materials[0];
    }

    private void Update()
    {
        _mat.SetVector(VariableNamesDefine.InvisibleWallPropertyName, Player.Instance.transform.position);
    }
}