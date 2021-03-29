using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;
using VLB;

public class PhotoCameraManager : MonoBehaviour
{
    public SteamVR_Action_Boolean ActionToggle = SteamVR_Input.GetBooleanAction("TriggerToggle");

    Interactable _interactable;

    [SerializeField] Light[] _cameraFlash;
    [SerializeField] float _flashDuration = 0.1f;
    [SerializeField] float _useCooldown = 2f;
    [SerializeField] Renderer _flashLampRend;
    [SerializeField] Camera _renderCamera;
    [SerializeField] GameObject _flashTriggerZone;

    bool _isTakingPicture = false;
    float _timer;

    bool _enemyIsInPicture = false;

    void Start()
    {
        _renderCamera.enabled = false;
        _flashTriggerZone.SetActive(false);
        _interactable = GetComponent<Interactable>();
        foreach (Light flash in _cameraFlash)
        {
            flash.enabled = false;
        }

        _flashLampRend.material.DisableKeyword("_EMISSION");
    }

    void Update()
    {
        if (_interactable.attachedToHand)
        {
            SteamVR_Input_Sources hand = _interactable.attachedToHand.handType;

            if (ActionToggle.GetStateDown(hand))
            {
                if (!_isTakingPicture)
                    _isTakingPicture = true;
            }
        }

        TakePicture();

    }

    void TakePicture()
    {
        if (_isTakingPicture)
        {
            _timer += Time.deltaTime;
            if (_timer < _flashDuration)
            {
                foreach (Light flash in _cameraFlash)
                {
                    flash.enabled = true;
                }
                _renderCamera.enabled = true;
                _flashTriggerZone.SetActive(true);
                _flashLampRend.material.EnableKeyword("_EMISSION");
            }
            else if (_timer > _flashDuration)
            {
                foreach (Light flash in _cameraFlash)
                {
                    flash.enabled = false;
                }   
                _renderCamera.enabled = false;
                _flashTriggerZone.SetActive(false);
                _flashLampRend.material.DisableKeyword("_EMISSION");
            }
            if (_timer >= _useCooldown)
            {
                _timer = 0f;
                _isTakingPicture = false;
            }
        }
        
    }

    public void SetEnemyIsInPicture(bool value)
    {
        _enemyIsInPicture = value;
    }

    public bool GetEnemyIsInPicture()
    {
        return _enemyIsInPicture;
    }
}
