using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VLB;

public class LampCollisionsWithEnemy : MonoBehaviour
{
    [SerializeField] Light[] _lightObjects;
    VolumetricLightBeam _lightBeam;
    [SerializeField] MeshRenderer _rend;
    [SerializeField] LightManager _lightManager;

    EnemyController _enemyController;

    // Lights flickering
    [SerializeField] float _flickerDelay = 1f;

    float[] _lightsOriginalIntensity;
    int _index = 0;
    bool _isFlickering = false;

    float[] _timer;

    void Start()
    {
        _lightBeam = GetComponent<VolumetricLightBeam>();
        _lightsOriginalIntensity = new float[_lightObjects.Length];

        _timer = new float[_lightObjects.Length];

        foreach (Light light in _lightObjects)
        {
            for (int i = _index; i <= _index; i++)
            {
                _lightsOriginalIntensity[i] = light.intensity;
            }
            _index++;
        }
        _index = 0;
    }

    void Update()
    {
        if (_isFlickering)
        {
            _index = 0;
            foreach (Light light in _lightObjects)
            {
                for (int i = _index; i <= _index; i++)
                {
                    _timer[i] += Time.deltaTime;
                    if (_timer[i] > _flickerDelay)
                    {
                        _timer[i] = 0;
                        if (light.intensity == _lightsOriginalIntensity[i])
                        {
                            light.intensity = 0;
                            _lightBeam.enabled = false;
                            _rend.material.DisableKeyword("_EMISSION");
                        }
                        else if (light.intensity == 0)
                        {
                            light.intensity = _lightsOriginalIntensity[i];
                            _lightBeam.enabled = true;
                            _rend.material.EnableKeyword("_EMISSION");
                        }
                    }
                }
                _index++;
            }
        }


        if (_enemyController != null && !_enemyController.GetNearLightSource())
        {
            StopFlickering();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy") && !_isFlickering && _lightManager.GetIsLightOn())
        {
            _enemyController = other.gameObject.GetComponent<EnemyController>();
            _enemyController.SetNearLightSource(true);
            _isFlickering = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy") && _isFlickering && _lightManager.GetIsLightOn())
        {
            StopFlickering();
        }
    }

    void StopFlickering()
    {
        _isFlickering = false;
        _enemyController.SetNearLightSource(false);

        _index = 0;
        foreach (Light light in _lightObjects)
        {
            for (int i = _index; i <= _index; i++)
            {
                light.intensity = _lightsOriginalIntensity[i];
                if (!_lightBeam.enabled)
                    _lightBeam.enabled = true;
                _rend.material.EnableKeyword("_EMISSION");
            }
            _index++;
        }
    }
}
