using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowHeadPosition : MonoBehaviour
{
    // TODO: Adjust rotation so that the belt rotates towards the position the player is looking
    [SerializeField] Transform _playerHead;
    //[SerializeField] Transform _playerBody;
    [SerializeField] float _moveStep = 0.1f;

    Vector3 MovePos;
    Quaternion MoveRotation;

    void Start()
    {
        
    }

    void Update()
    {
        MovePos = new Vector3(_playerHead.position.x, this.gameObject.transform.position.y, _playerHead.position.z);
        MoveRotation = new Quaternion(this.gameObject.transform.rotation.x, -_playerHead.rotation.y, this.gameObject.transform.rotation.z, 1);

        this.gameObject.transform.position = Vector3.MoveTowards(this.gameObject.transform.position, MovePos, _moveStep);
        //this.gameObject.transform.localRotation = _playerBody.rotation;

    }
}
