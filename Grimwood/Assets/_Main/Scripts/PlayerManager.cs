using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PlayerManager : MonoBehaviour
{
    PlayerMovement _playerMove;

    [SerializeField] float _defaultMoveSpeed = 3f;

    // Default post processing values
    float _saturationValueDefault;
    Color _colorFilterValueDefault;

    // Freezing post processing values
    [SerializeField] float _saturationValueFreeze = -80f;
    [SerializeField] Color _colorFilterValueFreeze;

    // Frost atmosphere values
    bool _isFreezing;
    bool _isUnfreezing;
    float _simulationSpeedFreeze = 250f;

    [SerializeField] float _frozenMoveSpeed = 2f;
    [SerializeField] GameObject _freezingParticles;
    [SerializeField] float _changeDurationFreeze;

    // Blinding atmosphere values
    bool _isBlinded;
    bool _isNotBlinded;
    bool _selectionCheck = false;

    float _simulationSpeedBlind = 25f;
    float _fogDensityDefault;
    float _fogDensityCurrent;
    [SerializeField] float _fogDensityValueTarget = 0.3f;
    [SerializeField] float _changeDurationFog;

    // General simulation values
    float _saturationValueCurrent;
    Color _colorFilterValueCurrent;


    float _timer = 0f;
    bool _isTimerStarted;
    bool _isTimerFinished = false;

    [SerializeField] Volume _ppVolume;
    ColorAdjustments _colAdj;

    void Start()
    {
        _isFreezing = false;
        _isUnfreezing = false;
        _isTimerStarted = false;
        _playerMove = GetComponent<PlayerMovement>();
        _freezingParticles.SetActive(false);

        _ppVolume.profile.TryGet<ColorAdjustments>(out _colAdj);
        _saturationValueDefault = _colAdj.saturation.value;
        _colorFilterValueDefault = _colAdj.colorFilter.value;

        _isBlinded = false;
        _isNotBlinded = false;

        _fogDensityDefault = RenderSettings.fogDensity;

        //_saturationValueCurrent = _saturationValueDefault;
        //_colorFilterValueCurrent = _colorFilterValueDefault;
    }

    void Update()
    {
        FreezingBehavior();
        BlindingBehavior();
        Debug.Log(_colorFilterValueDefault.ToString());

        //debug
        if (Input.GetKeyDown(KeyCode.Alpha6))
            _isBlinded = true;
        if (Input.GetKeyDown(KeyCode.Alpha7))
            _isBlinded = false;
    }

    void FreezingBehavior()
    {
        if (_isFreezing && !_freezingParticles.activeInHierarchy)
        {
            _playerMove.SetMaxSpeed(_frozenMoveSpeed);
            _freezingParticles.SetActive(true);
            Debug.Log("TurnedON");
            _isUnfreezing = false;
            _isTimerStarted = false;
            _isTimerFinished = false;

        }
        else if (!_isFreezing && _freezingParticles.activeInHierarchy)
        {
            _playerMove.SetMaxSpeed(_defaultMoveSpeed);
            _freezingParticles.SetActive(false);
            Debug.Log("TurnedOFF");
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
                Debug.Log("Stopped");
            }
        }
    }

    void BlindingBehavior()
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
            Debug.Log(_timer);
            if (_changeDurationFog <= _timer)
            {
                _isTimerFinished = true;
                Debug.Log("Stopped");
            }
        }
    }

    public bool GetIsFreezing()
    {
        return _isFreezing;
    }

    public void SetIsFreezing(bool value)
    {
        _isFreezing = value;
    }

    public bool GetIsBlinded()
    {
        return _isBlinded;
    }

    public void SetIsBlinded(bool value)
    {
        _isBlinded = value;
    }
}
