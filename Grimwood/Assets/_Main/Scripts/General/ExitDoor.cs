using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitDoor : MonoBehaviour
{
    Animator _anim;
    bool _isOpen = false;

    void Start()
    {
        _anim = GetComponent<Animator>();
    }

    void Update()
    {
        //Debug.Log(GameManager.instance.GetArtifactsCount());
        if (GameManager.instance.GetCollectedArtifactCount().Equals(GameManager.instance.GetArtifactsCount()) && !_isOpen)
        {
            AudioManager.instance.Play("ExitOpen", this.gameObject);
            _anim.SetBool("shouldOpen", true);
            _isOpen = true;
        }
    }
}
