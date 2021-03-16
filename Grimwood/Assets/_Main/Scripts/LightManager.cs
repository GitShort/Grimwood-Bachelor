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

    void Start()
    {
        TurnOffLight();
        hoverButton.onButtonDown.AddListener(OnButtonDown);
    }

    private void Update()
    {
        if (!GameManager.GetIsGeneratorOn())
            TurnOffLight();
    }

    void OnButtonDown(Hand hand)
    {
        if (!_isLightOn && GameManager.GetIsGeneratorOn())
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
        foreach (Light light in _lightObjects)
        {
            light.enabled = false;
        }
        _volumetricBeam.SetActive(false);
        _rend.material.DisableKeyword("_EMISSION");
        //Debug.Log("Turned off the light");
    }

}
