﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class AtmosphereBehavior : IEnemyBehavior
{
    PlayerMovement _playerMove;
    Transform _playerHead;

    // Default post processing values
    float _saturationValueDefault;
    Color _colorFilterValueDefault;

    // Freezing post processing values
    float _saturationValueFreeze = -80f;//
    Color _colorFilterValueFreeze;//

    // Frost atmosphere values
    bool _isFreezing;
    bool _isUnfreezing;
    float _simulationSpeedFreeze = 250f;

    float _frozenMoveSpeed = 2f;
    GameObject _freezingParticles;
    float _changeDurationFreeze;

    float _defaultMoveSpeed = 3f;

    float _timer = 0f;
    bool _isTimerStarted;
    bool _isTimerFinished = false;

    Volume _ppVolume;
    ColorAdjustments _colAdj;

    // General simulation values
    float _saturationValueCurrent;
    Color _colorFilterValueCurrent;

    Vector3 castsPosition;
    RaycastHit hit;
    LayerMask _includedLayersAtmosphere;
    float _atmosphereDistance;

    EnemyController _enemy;

    float _castsHeightOffset = 0.75f;

    public AtmosphereBehavior(BehaviorAttributes attributes)
    {
        _isFreezing = false;
        _isUnfreezing = false;
        _isTimerStarted = false;

        _saturationValueFreeze = attributes.GetSaturationValueFreeze();
        _colorFilterValueFreeze = attributes.GetColorFilterValueFreeze();
        _frozenMoveSpeed = attributes.GetFrozenMoveSpeed();
        _freezingParticles = attributes.GetFreezingParticles();
        _changeDurationFreeze = attributes.GetChangeDurationFreeze();
        _defaultMoveSpeed = attributes.GetDefaultMoveSpeed();
        _ppVolume = attributes.GetPPVolume();
        _playerHead = attributes.GetPlayerHead();
        _includedLayersAtmosphere = attributes.GetIncludedLayersAtmosphere();
        _atmosphereDistance = attributes.GetAtmosphereDistance();
        _playerMove = attributes.GetPlayerMove();
        _enemy = attributes.GetEnemyController();
        _freezingParticles.SetActive(false);

        _ppVolume.profile.TryGet<ColorAdjustments>(out _colAdj);
        _saturationValueDefault = _colAdj.saturation.value;
        _colorFilterValueDefault = _colAdj.colorFilter.value;
    }

    public void DebugFunction()
    {
        Debug.Log("WORKS");
    }

    public bool CheckState()
    {
        return _isFreezing;
    }

    public void SetState(bool value)
    {
        _isFreezing = value;
    }

    public void CallBehavior()
    {
        castsPosition = new Vector3(_enemy.transform.position.x, _enemy.transform.position.y + _castsHeightOffset, _enemy.transform.position.z);
        if (Physics.Linecast(castsPosition, _playerHead.position, out hit, _includedLayersAtmosphere, QueryTriggerInteraction.Ignore))
        {
            if (hit.distance <= _atmosphereDistance)
            {
                _isFreezing = true;
                Debug.Log("Player has entered FROST aura");
            }
            else
            {
                _isFreezing = false;
                Debug.Log("Player has left FROST aura");
            }
        }
    }

    public void Behavior()
    {
        if (_isFreezing && !_freezingParticles.activeInHierarchy)
        {
            _playerMove.SetMaxSpeed(_frozenMoveSpeed);
            _freezingParticles.SetActive(true);
            //Debug.Log("TurnedON");
            _isUnfreezing = false;
            _isTimerStarted = false;
            _isTimerFinished = false;

        }
        else if (!_isFreezing && _freezingParticles.activeInHierarchy)
        {
            _playerMove.SetMaxSpeed(_defaultMoveSpeed);
            _freezingParticles.SetActive(false);
            //Debug.Log("TurnedOFF");
            _isTimerStarted = false;
            _isUnfreezing = true;
            _isTimerFinished = false;
        }

        if (_isFreezing)
            FreezingPostProcessingEffect(_saturationValueFreeze, _colorFilterValueFreeze);
        if (_isUnfreezing)
            FreezingPostProcessingEffect(_saturationValueDefault, _colorFilterValueDefault);
        _saturationValueCurrent = _colAdj.saturation.value;
        _colorFilterValueCurrent = _colAdj.colorFilter.value;
    }



    void FreezingPostProcessingEffect(float saturationValue, Color colorFilterValue)
    {
        if (!_isTimerStarted && !_isTimerFinished)
        {
            _timer = 0f;
            _isTimerStarted = true;
        }
        if (_isTimerStarted && !_isTimerFinished)
        {
            _timer += Time.deltaTime / _simulationSpeedFreeze;
            _colAdj.saturation.value = Mathf.SmoothStep(_saturationValueCurrent, saturationValue, _timer);
            _colAdj.colorFilter.value = Color.Lerp(_colorFilterValueCurrent, colorFilterValue, _timer);
            if (_changeDurationFreeze <= _timer)
            {
                _isTimerFinished = true;
                //Debug.Log("Stopped");
            }
        }
    }
}
