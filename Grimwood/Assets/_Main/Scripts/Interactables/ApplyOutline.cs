using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class ApplyOutline : MonoBehaviour
{
    Interactable _interactable;
    Outline outline;

    bool _isObjectSnapped;

    // Start is called before the first frame update
    void Start()
    {
        _interactable = GetComponent<Interactable>();
        outline = GetComponent<Outline>();
        outline.enabled = true;
        _isObjectSnapped = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (_interactable.attachedToHand || _isObjectSnapped)
        {
            if (outline.enabled)
            {
                outline.enabled = false;
            }
        }
        else if (!_interactable.attachedToHand || !_isObjectSnapped)
        {
            if (!outline.enabled)
            {
                outline.enabled = true;
            }
        }
    }

    public bool GetIsObjectSnapped()
    {
        return _isObjectSnapped;
    }

    public void SetIsObjectSnapped(bool value)
    {
        _isObjectSnapped = value;
    }
}
