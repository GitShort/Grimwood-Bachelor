using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorHandleFollowPhysics : MonoBehaviour
{
    public Transform target;
    Rigidbody _rb;

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        _rb.MovePosition(target.transform.position);
    }
}
