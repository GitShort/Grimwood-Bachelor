using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutsideLampCollisionsWithEnemy : MonoBehaviour
{
    EnemyController _enemyController;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            _enemyController = other.gameObject.GetComponent<EnemyController>();
            _enemyController.SetNearLightSource(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            _enemyController.SetNearLightSource(false);
        }
    }

    private void OnDisable()
    {
        if(_enemyController != null)
            _enemyController.SetNearLightSource(false);
    }
}
