using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class BatterylevelTrigger : MonoBehaviour
{
    Interactable _interactable;
    FlashlightManager _flashlight;

    private void Start()
    {
        _flashlight = GetComponentInParent<FlashlightManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Battery"))
        {
            _interactable = other.gameObject.GetComponent<Interactable>();
            if (_interactable.attachedToHand)
            {
                _flashlight.SetCurrentBatteryLevel(_flashlight.GetBatteryRestoreValue());
                Destroy(other.gameObject);
            }
        }
    }
}
