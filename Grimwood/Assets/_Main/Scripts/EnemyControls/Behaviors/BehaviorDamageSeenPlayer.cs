using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class BehaviorDamageSeenPlayer : IEnemyBehavior
{
    // Called attributes
    EnemyController _enemy;
    NavMeshAgent _agent;
    float _changeDurationSeenDmg;

    Volume _ppVolume;
    ColorAdjustments _colAdj;
    Vignette _vignette;

    Transform _playerHead;

    Color _colorFilterValueSeenDmg;
    float _vignetteValueSeenDmg;
    // Local attributes
    bool _isSeen;
    bool _isNotSeen;
    float _simulationSpeed = 0.5f;

    float _t = 0f;
    bool _isTimerStarted;
    bool _isTimerFinished = false;
    bool _selectionCheck = false;
    float _tempTimer = 0f;

    Color _colorFilterValueDefault;
    float _vignetteValueDefault;

    Color _colorFilterValueCurrent;
    float _vignetteValueCurrent;

    bool _vignettePulsed = false;
    bool _vignetteFirstPulse = false;
    bool _vignetteSecondPulse = false;

    public BehaviorDamageSeenPlayer(AttributeStorage attributes)
    {
        _playerHead = attributes.GetPlayerHead();
        _enemy = attributes.GetEnemyController();
        _changeDurationSeenDmg = attributes.GetChangeDurationSeenDmg();
        _colorFilterValueSeenDmg = attributes.GetColorFilterValueSeenDmg();
        _vignetteValueSeenDmg = attributes.GetVignetteValueSeenDmg();


        _ppVolume = attributes.GetPPVolume();
        _ppVolume.profile.TryGet<ColorAdjustments>(out _colAdj);
        _ppVolume.profile.TryGet<Vignette>(out _vignette);
        
        _colorFilterValueDefault = _colAdj.colorFilter.value;
        _vignetteValueDefault = _vignette.intensity.value;
    }

    public void Behavior()
    {
        if (_isSeen && !_selectionCheck)
        {
            _isNotSeen = false;
            _isTimerStarted = false;
            _isTimerFinished = false;
            _selectionCheck = true;
        }
        else if (!_isSeen && _selectionCheck)
        {
            _isNotSeen = true;
            _isTimerStarted = false;
            _isTimerFinished = false;
            _selectionCheck = false;
        }

        if (_isSeen)
        {
            SeenPostProcessingEffect(_colorFilterValueSeenDmg);
            //AudioManager.instance.Play("Heartbeat", _playerHead.gameObject);
        }

        if (_isNotSeen)
        {
            SeenPostProcessingEffect(_colorFilterValueDefault);
            //AudioManager.instance.Stop("Heartbeat", _playerHead.gameObject);
        }

        _vignetteValueCurrent = _vignette.intensity.value;
        _colorFilterValueCurrent = _colAdj.colorFilter.value;

        if (_colorFilterValueCurrent.g <= 0.5f) // ~makes the player lose for staring at the monster for about 40 seconds :: FREEZING BEHAVIOR collides with these values!!! 
        {
            Debug.Log("Died by looking at the monster for too long!");
            GameManager.instance.SetIsPlayerAlive(false);
        }
        //else Debug.Log(_colorFilterValueCurrent.g);
    }

    public void CallBehavior()
    {
        if (_enemy.GetIsEnemyVisibleToPlayer() && !_isSeen)
        {
            _isSeen = true;
        }
        else if (!_enemy.GetIsEnemyVisibleToPlayer() && _isSeen)
        {
            _isSeen = false;
        }

        //debug
        if (Input.GetKeyDown(KeyCode.Alpha6))
            _isSeen = true;
        else if (Input.GetKeyDown(KeyCode.Alpha7))
            _isSeen = false;
    }

    public void DebugFunction()
    {
        Debug.Log("DAMAGE SEEN BY PLAYER behavior WORKS");
    }

    public string BehaviorMessage()
    {
        return "Dont look at it";
    }

    void SeenPostProcessingEffect(Color colorFilterValue)
    {
        if (!_isTimerStarted && !_isTimerFinished)
        {
            _t = 0f;
            _tempTimer = 0f;
            _isTimerStarted = true;
        }
        if (_isTimerStarted && !_isTimerFinished)
        {
            _t = Time.deltaTime * _simulationSpeed;
            _tempTimer += Time.deltaTime;

            _colAdj.colorFilter.value = Color.Lerp(_colorFilterValueCurrent, colorFilterValue, _t / 5f);
            AudioManager.instance.Play("Distortion", _playerHead.gameObject);
            AudioManager.instance.SetVolume("Distortion", 1f - _colAdj.colorFilter.value.g);

            //Debug.Log(_tempTimer);
            if (!_vignettePulsed)
            {
                if (!_vignetteFirstPulse)
                {
                    _vignette.intensity.value = Mathf.SmoothStep(_vignetteValueCurrent, _vignetteValueSeenDmg, _t * 40f);
                    if (_vignetteValueCurrent >= 0.85f)
                    {
                        _vignetteFirstPulse = true;
                        AudioManager.instance.Play("HeartbeatFirst", _playerHead.gameObject);
                    }
                }
                else if (_vignetteFirstPulse)
                {
                    _vignette.intensity.value = Mathf.SmoothStep(_vignetteValueCurrent, _vignetteValueDefault, _t * 15f);
                    if (_vignetteValueCurrent <= 0.6f)
                        _vignetteSecondPulse = true;
                }
                if (_vignetteSecondPulse)
                {
                    _vignette.intensity.value = Mathf.SmoothStep(_vignetteValueCurrent, _vignetteValueSeenDmg, _t * 40f);
                    if (_vignetteValueCurrent >= 0.85f)
                    {
                        _vignettePulsed = true;
                        AudioManager.instance.Play("HeartbeatSecond", _playerHead.gameObject);
                    }
                }
            }
            else if (_vignettePulsed)
            {
                _vignette.intensity.value = Mathf.SmoothStep(_vignetteValueCurrent, _vignetteValueDefault, _t * 15f);
                if (_vignetteValueCurrent <= 0.45f)
                {
                    _vignetteFirstPulse = false;
                    _vignetteSecondPulse = false;
                    _vignettePulsed = false;
                }
            }
            
            if (_changeDurationSeenDmg <= _tempTimer)
            {
                _isTimerFinished = true;
                _vignette.intensity.value = _vignetteValueDefault;
                //Debug.Log("Stopped");
            }

            if (_isNotSeen && !_isTimerFinished)
            {
                //Debug.Log(_colorFilterValueCurrent.r);
                _vignette.intensity.value = Mathf.SmoothStep(_vignetteValueCurrent, _vignetteValueDefault, _t * 10f);
                if (_vignetteValueCurrent <= 0.45f && _colorFilterValueCurrent.g >= _colorFilterValueDefault.g -0.015f)
                {
                    _isTimerFinished = true;
                    _vignette.intensity.value = _vignetteValueDefault;
                    _colAdj.colorFilter.value = _colorFilterValueDefault;
                }
            }
        }
    }
}
