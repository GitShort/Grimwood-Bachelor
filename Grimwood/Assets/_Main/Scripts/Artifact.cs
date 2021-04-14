using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;
using Valve.VR;

public class Artifact : MonoBehaviour
{
    Interactable _interactable;
    bool _isPickedUp;

    [SerializeField] GameObject _artifactObject;
    [SerializeField] ParticleSystem _pickupParticles;

    void Start()
    {
        _interactable = GetComponentInChildren<Interactable>();
        _isPickedUp = false;
    }

    void Update()
    {
        if (_interactable.attachedToHand && !_isPickedUp)
        {
            _isPickedUp = true;
            GameManager.instance.AddArtifactCollectedCount();
            _pickupParticles.Play();
            Destroy(_artifactObject);
            Invoke("DestroyParent", 1f);
        }
    }

    void DestroyParent()
    {
        Destroy(this.gameObject);
    }
}
