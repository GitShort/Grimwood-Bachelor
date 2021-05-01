using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowHeadPosition : MonoBehaviour
{
    [SerializeField] Transform _playerHead;

    [SerializeField] float _moveStep = 0.1f;
    [SerializeField] float _rotationDamping = 3f;

    Vector3 _movePos;
    Quaternion _moveRotation;
    Vector3 _lookPos;

    void Update()
    {
        _movePos = new Vector3(_playerHead.position.x, _playerHead.position.y - 0.5f, _playerHead.position.z);

        _lookPos = _playerHead.forward;
        _lookPos.y = 0;

        _moveRotation = Quaternion.LookRotation(_lookPos);
        this.gameObject.transform.rotation = Quaternion.Slerp(transform.rotation, _moveRotation, Time.deltaTime * _rotationDamping);

        this.gameObject.transform.position = Vector3.MoveTowards(this.gameObject.transform.position, _movePos, _moveStep);

    }
}
