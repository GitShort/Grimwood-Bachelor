using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;
using Valve.VR;
using TMPro;

public class Artifact : MonoBehaviour
{
    Interactable _interactable;
    bool _isPickedUp;

    [SerializeField] GameObject _artifactObject;
    [SerializeField] ParticleSystem _pickupParticles;

    [SerializeField] TextMeshPro _pickupText;
    [SerializeField] TextMeshPro _artifactText;

    void Start()
    {
        _interactable = GetComponentInChildren<Interactable>();
        _isPickedUp = false;
        _pickupText.gameObject.SetActive(false);
    }

    void Update()
    {
        if (_interactable.attachedToHand && !_isPickedUp)
        {
            _pickupText.gameObject.SetActive(true);
            _pickupText.text = _artifactText.text;
            _isPickedUp = true;
            GameManager.instance.AddCollectedArtifact(_artifactText.text);
            _pickupParticles.Play();
            Destroy(_artifactObject);
            Invoke("DestroyParent", 3f);
        }
    }

    void DestroyParent()
    {
        Destroy(this.gameObject);
    }
}
