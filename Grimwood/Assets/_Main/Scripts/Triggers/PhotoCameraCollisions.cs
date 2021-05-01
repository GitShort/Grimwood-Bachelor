using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotoCameraCollisions : MonoBehaviour
{
    PhotoCameraManager _cameraManager;

    private void Start()
    {
        _cameraManager = GetComponentInParent<PhotoCameraManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            _cameraManager.SetEnemyIsInPicture(true);
            Debug.Log("Enemy entered camera trigger");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            _cameraManager.SetEnemyIsInPicture(false);
            Debug.Log("Enemy left camera trigger");
        }
    }

    private void OnDisable()
    {
        if (_cameraManager.GetEnemyIsInPicture())
        {
            _cameraManager.SetEnemyIsInPicture(false);
            Debug.Log("Enemy left camera trigger");
        }
    }
}
