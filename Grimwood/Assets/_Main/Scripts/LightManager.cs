using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightManager : MonoBehaviour
{
    [SerializeField] Light[] _lightObjects;

    void Start()
    {
        foreach (Light light in _lightObjects)
        {
            light.enabled = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer.Equals(10))
        {
            foreach (Light light in _lightObjects)
            {
                light.enabled = true;
            }
            Debug.Log("ENTERED");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer.Equals(10))
        {
            foreach (Light light in _lightObjects)
            {
                light.enabled = false;
            }
            Debug.Log("LEFT");
        }
    }

}
