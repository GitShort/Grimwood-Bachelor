using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitDoor : MonoBehaviour
{
    Animator _anim;
    bool _isOpen = false;

    // Start is called before the first frame update
    void Start()
    {
        _anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(GameManager.instance.GetArtifactsCount());
        if (GameManager.instance.GetCollectedArtifactCount().Equals(GameManager.instance.GetArtifactsCount()) && !_isOpen)
        {
            AudioManager.instance.Play("ExitOpen", this.gameObject);
            _anim.SetBool("shouldOpen", true);
            _isOpen = true;
        }
    }
}
