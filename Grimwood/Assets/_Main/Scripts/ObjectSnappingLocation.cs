using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class ObjectSnappingLocation : MonoBehaviour
{
    Rigidbody _rb;
    Interactable _interactable;
    ApplyOutline _outline;

    bool _isSnapped = false;
    string _objectNameEnter = "";

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Snappable") && !_isSnapped)
        {
            _interactable = other.gameObject.GetComponent<Interactable>();
            Debug.Log("NEAR");
            if (!_interactable.attachedToHand && _objectNameEnter.Equals(""))
            {
                _outline = other.gameObject.GetComponent<ApplyOutline>();
                _rb = other.gameObject.GetComponent<Rigidbody>();
                _rb.useGravity = false;
                _rb.constraints = RigidbodyConstraints.FreezeAll;
                Debug.Log("snapped");
                ChangeToParent(this.gameObject.transform, other.gameObject);
                _objectNameEnter = other.gameObject.name;
                _isSnapped = true;
                _outline.SetIsObjectSnapped(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Snappable"))
        {
            if (_objectNameEnter.Equals(other.gameObject.name) && _isSnapped)
            {
                ChangeToWorld();
                _outline = other.gameObject.GetComponent<ApplyOutline>();
                _rb = other.gameObject.GetComponent<Rigidbody>();
                _rb.useGravity = true;
                _rb.constraints = RigidbodyConstraints.None;
                Debug.Log("removed");
                _objectNameEnter = "";
                _isSnapped = false;
                _outline.SetIsObjectSnapped(false);
            }
        }
    }

    void ChangeToParent(Transform newParent, GameObject snappedObject)
    {
        snappedObject.transform.SetParent(newParent);
        Debug.Log("Applied parent");
        snappedObject.transform.localPosition = Vector3.zero;
        snappedObject.transform.localRotation = Quaternion.identity;
    }

    void ChangeToWorld()
    {
        this.gameObject.transform.DetachChildren();
        //snappedObject.transform.parent = null;
        Debug.Log("Removed from parent");
    }
}
