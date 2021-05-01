using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (GameManager.instance.GetCollectedArtifactCount().Equals(GameManager.instance.GetArtifactsCount()) && other.gameObject.CompareTag("Player") && !GameManager.instance.GetIsFinished())
        {
            GameManager.instance.SetIsFinished(true);
        }
        GameManager.instance.SetIsFinished(true);
    }
}
