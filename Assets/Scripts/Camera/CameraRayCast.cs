using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class CameraRayCast : MonoBehaviour
{
    [SerializeField, Required] private LayerMask _obstacleLayer;
    private readonly List<TransparencyController> _currentObstacles = new();
    private readonly RaycastHit[] _raycastHits = new RaycastHit[10];

    private void LateUpdate()
    {
        Debug.DrawLine(transform.position, Player.Instance.transform.position, Color.red);
        FadeOutOldObstacles();
        FadeInNewObstacles();
    }

    private void FadeOutOldObstacles()
    {
        foreach (var obstacle in _currentObstacles)
        {
            obstacle.FadeOut();
        }

        _currentObstacles.Clear();
    }

    private void FadeInNewObstacles()
    {
        var castDir = Player.Instance.transform.position - transform.position;
        var hitsNum = Physics.RaycastNonAlloc(
            transform.position,
            castDir,
            _raycastHits,
            castDir.magnitude,
            _obstacleLayer);

        for (var i = 0; i < hitsNum; i++)
        {
            var hit = _raycastHits[i];

            if (!hit.transform.TryGetComponent<TransparencyController>(out var controller)) continue;
            _currentObstacles.Add(controller);
            controller.FadeIn();
        }
    }
}