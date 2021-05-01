using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float _gravity = 120f;
    [SerializeField] float _rotateIncrement = 30f;

    [SerializeField] float _sensitivity = 0.1f;
    [SerializeField] float _maxSpeed = 4f;

    [SerializeField] SteamVR_Action_Boolean SnapLeftAction = SteamVR_Input.GetBooleanAction("SnapTurnLeft");
    [SerializeField] SteamVR_Action_Boolean SnapRightAction = SteamVR_Input.GetBooleanAction("SnapTurnRight");
    [SerializeField] SteamVR_Action_Vector2 MoveValue = SteamVR_Input.GetVector2Action("TouchpadPosition");
    [SerializeField] SteamVR_Action_Boolean PauseAction = SteamVR_Input.GetBooleanAction("PauseMenu");

    [SerializeField] GameObject _pauseMenu;

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
        HandlePauseMenu();
        if (GameManager.instance.GetIsPlayerAlive() && !GameManager.instance.GetIsPaused() && !GameManager.instance.GetIsFinished())
        {
            CalculateMovement();
            SnapRotation();
        }
    }

    void HandleHeight()
    {
        float _headHeight = Mathf.Clamp(_head.localPosition.y, 1, 2);
        _charController.height = _headHeight;

        Vector3 _newCenter = Vector3.zero;
        _newCenter.y = _charController.height / 2;
        _newCenter.y += _charController.skinWidth;

        _newCenter.x = _head.localPosition.x;
        _newCenter.z = _head.localPosition.z;

        _charController.center = _newCenter;
    }

    void HandlePauseMenu()
    {
        if (GameManager.instance.GetIsPlayerAlive() && !SceneManager.GetActiveScene().name.Equals("Main Menu"))
        {
            if (PauseAction.GetStateDown(SteamVR_Input_Sources.RightHand) || PauseAction.GetStateDown(SteamVR_Input_Sources.LeftHand))
            {
                if (!GameManager.instance.GetIsPaused())
                {
                    GameManager.instance.SetIsPaused(true);
                    _pauseMenu.SetActive(true);
                    Debug.Log("PAUSED");
                }
                else
                {
                    GameManager.instance.SetIsPaused(false);
                    _pauseMenu.SetActive(false);
                    Debug.Log("RESUMED");
                }
            }
        }

    }

    void CalculateMovement()
    {
        Quaternion _orientation = CalculateOrientation();
        Vector3 _movement = Vector3.zero;

        if (MoveValue.axis.magnitude == 0)
        {
            _speed = 0;
        }

        _speed += MoveValue.axis.magnitude * _sensitivity;
        _speed = Mathf.Clamp(_speed, -_maxSpeed, _maxSpeed);

        _movement += _orientation * (_speed * Vector3.forward);
        _movement.y -= _gravity * Time.deltaTime;

        _charController.Move(_movement * Time.deltaTime);

        //if (_charController.velocity != Vector3.zero && !SceneManager.GetActiveScene().name.Equals("Main Menu"))
        //{
        //    AudioManager.instance.Play("PlayerFootsteps", this.gameObject);
        //}
        //else if (_charController.velocity == Vector3.zero && !SceneManager.GetActiveScene().name.Equals("Main Menu"))
        //    AudioManager.instance.Stop("PlayerFootsteps", this.gameObject);
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
            _snapValue = -Mathf.Abs(_rotateIncrement);

        if (SnapRightAction.GetStateDown(SteamVR_Input_Sources.RightHand))
            _snapValue = Mathf.Abs(_rotateIncrement);

        transform.RotateAround(this.gameObject.transform.position, Vector3.up, _snapValue); // adjust to RotateAround(_head.position) later
    }

    public float GetMaxSpeed()
    {
        return _maxSpeed;
    }

    public void SetMaxSpeed(float value)
    {
        _maxSpeed = value;
    }
}
