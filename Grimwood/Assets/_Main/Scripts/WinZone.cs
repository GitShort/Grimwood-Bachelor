using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinZone : MonoBehaviour
{
    bool _hasPassedWinZone;
    [SerializeField] GameObject _textObj;

    private void Start()
    {
        _textObj.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (GameManager.instance.GetArtifactCollectedCount().Equals(1) && other.gameObject.CompareTag("Player"))
        {
            _hasPassedWinZone = true;
            _textObj.SetActive(true);
        }
    }

    public bool GetHasPassedWinZone()
    {
        return _hasPassedWinZone;
    }
}
