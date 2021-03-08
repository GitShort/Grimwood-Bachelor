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

    bool isLightOn = false;

    void Start()
    {
        TurnOffLight();
        hoverButton.onButtonDown.AddListener(OnButtonDown);
    }

    void OnButtonDown(Hand hand)
    {
        if (!isLightOn)
            TurnOnLight();
        else
            TurnOffLight();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer.Equals(10) && isLightOn)
        {
            TurnOnLight();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer.Equals(10) && !isLightOn)
        {
            TurnOffLight();
            Debug.Log("Light out of range and turned off");
        }
        else if (other.gameObject.layer.Equals(10) && isLightOn)
        {
            foreach (Light light in _lightObjects)
            {
                light.enabled = false;
            }
            _volumetricBeam.SetActive(true);
            _rend.material.EnableKeyword("_EMISSION");
            Debug.Log("Light out of range and turned on");
        }
    }
    void TurnOnLight()
    {
        isLightOn = true;
        foreach (Light light in _lightObjects)
        {
            light.enabled = true;
        }
        _volumetricBeam.SetActive(true);
        _rend.material.EnableKeyword("_EMISSION");
        Debug.Log("Turned on the light");
    }

    void TurnOffLight()
    {
        isLightOn = false;
        foreach (Light light in _lightObjects)
        {
            light.enabled = false;
        }
        _volumetricBeam.SetActive(false);
        _rend.material.DisableKeyword("_EMISSION");
        Debug.Log("Turned off the light");
    }

}
