using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateToPlayer : MonoBehaviour
{

    [SerializeField] Transform _playerHead;

    Quaternion _moveRotation;
    Vector3 _lookPos;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _lookPos = _playerHead.forward;
        _lookPos.y = 0;

        _moveRotation = Quaternion.LookRotation(_playerHead.forward);
        this.gameObject.transform.rotation = Quaternion.Slerp(transform.rotation, _moveRotation, Time.deltaTime);
    }
}
