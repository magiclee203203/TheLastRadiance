using Sirenix.OdinInspector;
using UnityEngine;

public class EnergyBallLightFade : MonoBehaviour
{
    [SerializeField, Required] [InfoBox("Seconds to dim the light")]
    private float _life = 0.2f;

    private Light _li;
    private float _initIntensity;

    private void Start()
    {
        _li = GetComponent<Light>();
        if (_li != null)
            _initIntensity = _li.intensity;
    }

    private void Update()
    {
        if (_li == null) return;
        _li.intensity -= _initIntensity * (Time.deltaTime / _life);

        if (!(_li.intensity <= 0f)) return;
        _li.enabled = false;
    }
}