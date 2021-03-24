using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class LightManager : MonoBehaviour
{
    [SerializeField] Light[] _lightObjects;
    [SerializeField] GameObject _volumetricBeam;

    [SerializeField] HoverButton hoverButton;
    [SerializeField] MeshRenderer _rend;

    bool _isLightOn = false;

    // Lights flickering
    [SerializeField] float _flickerDelay = 1f;

    float[] _lightsOriginalIntensity;
    int _index = 0;
    bool _isFlickering = false;

    float[] _timer;

    void Start()
    {
        TurnOffLight();
        hoverButton.onButtonDown.AddListener(OnButtonDown);

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

    private void Update()
    {
        if (!GameManager.instance.GetIsGeneratorOn())
            TurnOffLight();

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
                            _volumetricBeam.SetActive(false);
                            _rend.material.DisableKeyword("_EMISSION");
                        }
                        else if (light.intensity == 0)
                        {
                            light.intensity = _lightsOriginalIntensity[i];
                            _volumetricBeam.SetActive(true);
                            _rend.material.EnableKeyword("_EMISSION");
                        }
                    }
                }
                _index++;
            }
        }
    }

    void OnButtonDown(Hand hand)
    {
        if (!_isLightOn && GameManager.instance.GetIsGeneratorOn())
            TurnOnLight();
        else
            TurnOffLight();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer.Equals(10) && _isLightOn)
        {
            TurnOnLight();
        }
        if (other.gameObject.CompareTag("Enemy") && !_isFlickering && _isLightOn)
        {
            _isFlickering = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer.Equals(10) && !_isLightOn)
        {
            TurnOffLight();
            //Debug.Log("Light out of range and turned off");
        }
        else if (other.gameObject.layer.Equals(10) && _isLightOn)
        {
            foreach (Light light in _lightObjects)
            {
                light.enabled = false;
            }
            _volumetricBeam.SetActive(true);
            _rend.material.EnableKeyword("_EMISSION");
            //Debug.Log("Light out of range and turned on");
        }

        if (other.gameObject.CompareTag("Enemy") && _isFlickering && _isLightOn)
        {
            _isFlickering = false;

            _index = 0;
            foreach (Light light in _lightObjects)
            {
                for (int i = _index; i <= _index; i++)
                {
                    light.intensity = _lightsOriginalIntensity[i];
                    if (!_volumetricBeam.activeInHierarchy)
                        _volumetricBeam.SetActive(true);
                    _rend.material.EnableKeyword("_EMISSION");
                }
                _index++;
            }
        }
    }
    void TurnOnLight()
    {
        _isLightOn = true;
        foreach (Light light in _lightObjects)
        {
            light.enabled = true;
        }
        _volumetricBeam.SetActive(true);
        _rend.material.EnableKeyword("_EMISSION");
        //Debug.Log("Turned on the light");
    }

    void TurnOffLight()
    {
        _isLightOn = false;
        _isFlickering = false;
        foreach (Light light in _lightObjects)
        {
            light.enabled = false;
        }
        _volumetricBeam.SetActive(false);
        _rend.material.DisableKeyword("_EMISSION");
        //Debug.Log("Turned off the light");
    }

}
