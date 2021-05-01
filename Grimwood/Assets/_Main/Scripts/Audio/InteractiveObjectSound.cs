using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class InteractiveObjectSound : MonoBehaviour
{
    Interactable _interactable;

    bool _pickupSoundPlayed;

    private void Start()
    {
        _interactable = GetComponent<Interactable>();
    }

    private void Update()
    {
        if (_interactable.attachedToHand && !_pickupSoundPlayed)
        {
            _pickupSoundPlayed = true;
            AudioManager.instance.Play("ObjectPickup", this.gameObject);
        }
        else if (!_interactable.attachedToHand && _pickupSoundPlayed)
        {
            _pickupSoundPlayed = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer != 9 && !_interactable.attachedToHand)
        {
            AudioManager.instance.Play("ObjectFall", this.gameObject);
        }
    }
}
