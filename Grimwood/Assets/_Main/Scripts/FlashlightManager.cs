using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;
using VLB;

public class FlashlightManager : MonoBehaviour
{
    public SteamVR_Action_Boolean ActionToggle = SteamVR_Input.GetBooleanAction("TriggerToggle");

    Interactable _interactable;
    EnemyController _enemy;

    [SerializeField] GameObject _lightObjs;
    [SerializeField] float _lightRange = 6f;
    [SerializeField] VolumetricLightBeam _lightBeam;
    [SerializeField] LayerMask _layerMask;
    RaycastHit hit;
    Ray ray;

    [SerializeField] float _batteryConsumptionRate = 0.1f;
    [SerializeField] Renderer[] _BatteryIndicators;
    float[] _batteryIndicatorsValue;
    [SerializeField] float _maxBatteryLevel = 4f;
    float _currentBatteryLevel;
    [SerializeField] float _batteryRestoreValue = 2f;


    void Start()
    {
        _interactable = GetComponent<Interactable>();
        _lightObjs.SetActive(false);
        _currentBatteryLevel = _maxBatteryLevel;

        _batteryIndicatorsValue = new float[_BatteryIndicators.Length];
        for (int i = 0; i < _batteryIndicatorsValue.Length; i++)
        {
            _batteryIndicatorsValue[i] = i + 1;
        }
    }

    void Update()
    {
        FlashlightTriggerAction();
        if (_currentBatteryLevel < 0)
        {
            _currentBatteryLevel = 0;
            if(_lightObjs.activeInHierarchy)
                _lightObjs.SetActive(false);
        }
            
    }

    void FlashlightTriggerAction()
    {
        if (_interactable.attachedToHand)
        {
            SteamVR_Input_Sources hand = _interactable.attachedToHand.handType;

            if (ActionToggle.GetStateDown(hand))
            {
                if (!_lightObjs.activeInHierarchy && _currentBatteryLevel > 0)
                    _lightObjs.SetActive(true);
                else
                    _lightObjs.SetActive(false);
            }
        }
        FlashlightBeam();
    }

    void FlashlightBeam()
    {
        if (_lightObjs.activeInHierarchy && _currentBatteryLevel >= 0)
        {
            BatteryLevel();
            ray = new Ray(transform.position, transform.forward);
            if (Physics.Raycast(ray, out hit, _lightRange, _layerMask))
            {
                _lightBeam.fallOffEnd = hit.distance;
                _lightBeam.UpdateAfterManualPropertyChange();

                if (hit.collider.gameObject.CompareTag("Enemy"))
                {
                    _enemy = hit.collider.GetComponent<EnemyController>();
                    _enemy.SetIsCollidingWithFlashlight(true); // perdaryti i interface 
                }
                else if (!hit.collider.gameObject.CompareTag("Enemy") && _enemy != null)
                {
                    _enemy.SetIsCollidingWithFlashlight(false);
                }
            }
            else if (_enemy != null && _enemy.GetIsCollidingWithFlashlight())
                _enemy.SetIsCollidingWithFlashlight(false);
        }
        else if (_enemy != null)
            _enemy.SetIsCollidingWithFlashlight(false);    
    }

    void BatteryLevel()
    {
        _currentBatteryLevel -= Time.deltaTime * _batteryConsumptionRate;
        for (int i = 0; i < _batteryIndicatorsValue.Length; i++)
        {
            if (_currentBatteryLevel < _batteryIndicatorsValue[i] && _BatteryIndicators[i].material.IsKeywordEnabled("_EMISSION"))
            {
                Debug.Log(_batteryIndicatorsValue[i].ToString() + " used");
                _BatteryIndicators[i].material.DisableKeyword("_EMISSION");
            }
            else if (_currentBatteryLevel > _batteryIndicatorsValue[i] && !_BatteryIndicators[i].material.IsKeywordEnabled("_EMISSION"))  
            {
                Debug.Log(_batteryIndicatorsValue[i].ToString() + " restored");
                _BatteryIndicators[i].material.EnableKeyword("_EMISSION");
            }
        }
        Debug.Log("Battery Level: " + _currentBatteryLevel.ToString());
    }

    public void SetCurrentBatteryLevel(float value)
    {
        if ((_currentBatteryLevel > _maxBatteryLevel-2f && _currentBatteryLevel < _maxBatteryLevel-1f) || _currentBatteryLevel >= _maxBatteryLevel)
        {
            _currentBatteryLevel = _maxBatteryLevel;
        }
        else
            _currentBatteryLevel += value;

        for (int i = 0; i < _batteryIndicatorsValue.Length; i++)
        {
            Debug.Log("FOR");
            if (_currentBatteryLevel >= _batteryIndicatorsValue[i] && !_BatteryIndicators[i].material.IsKeywordEnabled("_EMISSION"))
            {
                _BatteryIndicators[i].material.EnableKeyword("_EMISSION");
                Debug.Log(_batteryIndicatorsValue[i].ToString() + " restored");
            }
        }
    }

    public float GetCurrentBatteryLevel()
    {
        return _currentBatteryLevel;
    }

    public float GetBatteryRestoreValue()
    {
        return _batteryRestoreValue;
    }
}
