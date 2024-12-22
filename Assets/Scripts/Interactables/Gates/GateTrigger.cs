using System;
using UnityEngine;

public class GateTrigger : MonoBehaviour
{
    [HideInInspector] public Action Callback;

    private void OnTriggerEnter(Collider other)
    {
        Callback?.Invoke();
    }
}