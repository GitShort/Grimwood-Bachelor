using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightBeamCollisions : MonoBehaviour
{
    [SerializeField] Light[] _lights;
    [SerializeField] float _flickerDelay = 1f;

    float[] _lightsOriginalIntensity;
    int _index = 0;
    bool _isFlickering = false;

    float[] _timer;

    void Start()
    {
        _lightsOriginalIntensity = new float[_lights.Length];

        _timer = new float[_lights.Length];

        foreach (Light light in _lights)
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
        //Debug.Log(_lightsOriginalIntensity.Length);
        //foreach (float light in _lightsOriginalIntensity)
        //{
        //    Debug.Log(light);
        //}

        if (_isFlickering)
        {
            _index = 0;
            foreach (Light light in _lights)
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
                        }
                        else if (light.intensity == 0)
                        {
                            light.intensity = _lightsOriginalIntensity[i];
                        }
                    }
                }
                _index++;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy") && !_isFlickering)
        {
            _isFlickering = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy") && _isFlickering)
        {
            _isFlickering = false;

            _index = 0;
            foreach (Light light in _lights)
            {
                for (int i = _index; i <= _index; i++)
                {
                    light.intensity = _lightsOriginalIntensity[i];
                }
                _index++;
            }
        }
    }
}
