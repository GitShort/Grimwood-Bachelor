using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class VRSlidingMovement : MonoBehaviour
{
    public float Gravity = 60f;
    public float RotateIncrement = 90;

    public float Sensitivity = 0.1f;
    public float MaxSpeed = 1f;

    public SteamVR_Action_Boolean SnapLeftAction = SteamVR_Input.GetBooleanAction("SnapTurnLeft");
    public SteamVR_Action_Boolean SnapRightAction = SteamVR_Input.GetBooleanAction("SnapTurnRight");
    public SteamVR_Action_Vector2 MoveValue = SteamVR_Input.GetVector2Action("TouchpadPosition");

    float _speed = 0f;

    CharacterController _charController = null;
    [SerializeField] Transform _head;

    private void Awake()
    {
        _charController = GetComponent<CharacterController>();
    }

    void Start()
    {
        _charController.detectCollisions = false;
    }

    void Update()
    {
        HandleHeight();
        CalculateMovement();
        SnapRotation();
    }

    //void SnapCharacterController()
    //{
    //    float distanceFromFloor = Vector3.Dot(_head.localPosition, Vector3.up);
    //    _charController.height = Mathf.Max(_charController.radius, distanceFromFloor);
    //    _charController.center = _head.localPosition - 0.5f * distanceFromFloor * Vector3.up;
    //}

    void HandleHeight()
    {
        // Get the head in the local space
        float _headHeight = Mathf.Clamp(_head.localPosition.y, 1, 2);
        _charController.height = _headHeight;

        // Cut in half
        Vector3 _newCenter = Vector3.zero;
        _newCenter.y = _charController.height / 2;
        _newCenter.y += _charController.skinWidth;

        // Move capsule in local space
        _newCenter.x = _head.localPosition.x;
        _newCenter.z = _head.localPosition.z;

        // Apply
        _charController.center = _newCenter;
    }

    void CalculateMovement()
    {
        // Movement orientation
        Quaternion _orientation = CalculateOrientation();
        Vector3 _movement = Vector3.zero;

        // If player is not moving
        if (MoveValue.axis.magnitude == 0)
        {
            _speed = 0;
        }

        // Add, clamp
        _speed += MoveValue.axis.magnitude * Sensitivity;
        _speed = Mathf.Clamp(_speed, -MaxSpeed, MaxSpeed);

        // Orientation
        _movement += _orientation * (_speed * Vector3.forward);

        // Gravity
        _movement.y -= Gravity * Time.deltaTime;

        // Apply
        _charController.Move(_movement * Time.deltaTime);

    }

    Quaternion CalculateOrientation()
    {
        float _rotation = Mathf.Atan2(MoveValue.axis.x, MoveValue.axis.y);
        _rotation *= Mathf.Rad2Deg;

        Vector3 _orientationEuler = new Vector3(0, _head.eulerAngles.y + _rotation, 0);
        return Quaternion.Euler(_orientationEuler);
    }

    void SnapRotation()
    {
        float _snapValue = 0f;

        if (SnapLeftAction.GetStateDown(SteamVR_Input_Sources.RightHand))
            _snapValue = -Mathf.Abs(RotateIncrement);

        if (SnapRightAction.GetStateDown(SteamVR_Input_Sources.RightHand))
            _snapValue = Mathf.Abs(RotateIncrement);

        transform.RotateAround(this.gameObject.transform.position, Vector3.up, _snapValue); // adjust to RotateAround(_head.position) later
    }
}
