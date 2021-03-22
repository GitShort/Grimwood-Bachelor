using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotoCameraCollisions : MonoBehaviour
{
    EnemyController _enemy;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            _enemy = other.GetComponent<EnemyController>();
            _enemy.SetIsCollidingWithCamera(true);
            Debug.Log("Enemy entered camera trigger");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            _enemy = other.GetComponent<EnemyController>();
            _enemy.SetIsCollidingWithCamera(false);
            Debug.Log("Enemy left camera trigger");
        }
    }
}
