using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviorBlind : IEnemyBehavior
{
    // Called attributes
    Transform _playerHead;
    float _fogDensityValueTarget = 0.3f;
    float _changeDurationBlind;
    EnemyController _enemy;

    LayerMask _includedLayersAtmosphere;
    float _atmosphereDistance;
    float _castsHeightOffset;

    // Local attributes
    bool _isBlinded;
    bool _isNotBlinded;
    bool _selectionCheck = false;

    float _fogDensityDefault;
    float _fogDensityCurrent;
    float _simulationSpeedBlind = 25f;

    float _timer = 0f;
    bool _isTimerStarted;
    bool _isTimerFinished = false;

    Vector3 castsPosition;
    RaycastHit hit;

    public BehaviorBlind(AttributeStorage attributes)
    {
        _isBlinded = false;
        _isNotBlinded = false;
        _isTimerStarted = false;

        _fogDensityDefault = RenderSettings.fogDensity;


        _playerHead = attributes.GetPlayerHead();
        _fogDensityValueTarget = attributes.GetFogDensityValueTarget();
        _changeDurationBlind = attributes.GetChangeDurationBlind();
        _enemy = attributes.GetEnemyController();
        _fogDensityValueTarget = attributes.GetFogDensityValueTarget();
        _changeDurationBlind = attributes.GetChangeDurationBlind();
        _includedLayersAtmosphere = attributes.GetIncludedLayersAtmosphere();
        _atmosphereDistance = attributes.GetAtmosphereDistance();
        _castsHeightOffset = attributes.GetCastsHeightOffset();

    }

    public void DebugFunction()
    {
        Debug.Log("BLIND behavior WORKS");
    }

    public bool CheckState()
    {
        return _isBlinded;
    }

    public void SetState(bool value)
    {
        _isBlinded = value;
    }

    public void CallBehavior()
    {
        castsPosition = new Vector3(_enemy.transform.position.x, _enemy.transform.position.y + _castsHeightOffset, _enemy.transform.position.z);
        if (Physics.Linecast(castsPosition, _playerHead.position, out hit, _includedLayersAtmosphere, QueryTriggerInteraction.Ignore))
        {
            if (hit.distance <= _atmosphereDistance)
            {
                _isBlinded = true;
                Debug.Log("Player has entered BLIND atmosphere");
            }
            else
            {
                _isBlinded = false;
                Debug.Log("Player has left BLIND atmosphere");
            }
        }
    }

    public void Behavior()
    {
        if (_isBlinded && !_selectionCheck)
        {
            _isNotBlinded = false;
            _isTimerStarted = false;
            _isTimerFinished = false;
            _selectionCheck = true;
        }
        else if (!_isBlinded && _selectionCheck)
        {
            _isNotBlinded = true;
            _isTimerStarted = false;
            _isTimerFinished = false;
            _selectionCheck = false;
        }

        if (_isBlinded)
        {
            BlindingEffect(_fogDensityValueTarget);
        }

        if (_isNotBlinded)
            BlindingEffect(_fogDensityDefault);
        _fogDensityCurrent = RenderSettings.fogDensity;
    }

    void BlindingEffect(float densityValue)
    {
        if (!_isTimerStarted && !_isTimerFinished)
        {
            _timer = 0f;
            _isTimerStarted = true;
        }
        if (_isTimerStarted && !_isTimerFinished)
        {
            _timer += Time.deltaTime / _simulationSpeedBlind;
            RenderSettings.fogDensity = Mathf.SmoothStep(_fogDensityCurrent, densityValue, _timer);
            //Debug.Log(_timer);
            if (_changeDurationBlind <= _timer)
            {
                _isTimerFinished = true;
                //Debug.Log("Stopped");
            }
        }
    }


}
