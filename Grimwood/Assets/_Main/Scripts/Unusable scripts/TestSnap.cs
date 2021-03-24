using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TestSnap : MonoBehaviour
{
    Rigidbody _rb;
    //Interactable _interactable;
    bool _isSnapped = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Snappable") && !_isSnapped)
        {
            _rb = other.gameObject.GetComponent<Rigidbody>();
            _rb.useGravity = false;
            _rb.constraints = RigidbodyConstraints.FreezeAll;
            Debug.Log("snapped");
            _isSnapped = true;
            ChangeToParent(this.gameObject.transform, other.gameObject);

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Snappable"))
        {
            ChangeToWorld(other.gameObject);
            _rb = other.gameObject.GetComponent<Rigidbody>();
            _rb.useGravity = true;
            _rb.constraints = RigidbodyConstraints.None;
            _isSnapped = false;
            Debug.Log("removed");
            //ChangeToWorld(this.gameObject.transform, other.gameObject);
        }
    }

    void ChangeToParent(Transform newParent, GameObject snappedObject)
    {
        snappedObject.transform.SetParent(newParent);
        Debug.Log("Applied parent");
        snappedObject.transform.localPosition = Vector3.zero;
        snappedObject.transform.localRotation = Quaternion.identity;
    }

    void ChangeToWorld(GameObject snappedObject)
    {
        this.gameObject.transform.DetachChildren();
        //snappedObject.transform.parent = null;
        Debug.Log("Removed from parent");
    }
}
