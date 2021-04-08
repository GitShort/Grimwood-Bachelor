using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BehaviorLightWeak : IEnemyBehavior
{
    // Called attributes
    float _flashlightMoveSpeed;
    float _lampMoveSpeed;
    float _defaultMoveSpeed;
    Animator _anim;
    NavMeshAgent _agent;
    FlashlightManager _flashlight;
    EnemyController _enemyController;

    // Local attributes
    bool _isLitUp = false;
    bool _isLitUpLamp = false;

    bool _sentGeneratorSignal = false;

    public BehaviorLightWeak(AttributeStorage attributes)
    {
        _enemyController = attributes.GetEnemyController();
        _flashlightMoveSpeed = attributes.GetFlashlightEnemyMoveSpeedWeak();
        _lampMoveSpeed = attributes.GetLampEnemyMoveSpeedWeak();
        _defaultMoveSpeed = attributes.GetDefaultEnemyMoveSpeed();
        _anim = attributes.GetEnemyController().GetComponent<Animator>();
        _agent = attributes.GetEnemyController().GetComponent<NavMeshAgent>();
        _flashlight = attributes.GetFlashlightManager();
    }

    public void Behavior()
    {
        FlashlightBehavior();
        LampBehavior();
    }

    public void CallBehavior()
    {
        if (GameManager.instance.GetIsGeneratorOn() && !_sentGeneratorSignal)
        {
            _sentGeneratorSignal = true;
            _enemyController.SetGoToGenerator(true);
        }
        else if (!GameManager.instance.GetIsGeneratorOn() && _sentGeneratorSignal)
        {
            _sentGeneratorSignal = false;
            _enemyController.SetGoToGenerator(false);
        }
    }

    public bool CheckState()
    {
        return _isLitUp;
    }

    public void DebugFunction()
    {
        Debug.Log("FLASHLIGHT weak behavior WORKS");
    }

    public void SetState(bool value)
    {
        _isLitUp = value;
    }

    void FlashlightBehavior()
    {
        if (_flashlight.GetIsCollidingWithEnemy() && !_isLitUp)
        {
            _anim.Play("GetHitLight1");
            _agent.speed = _flashlightMoveSpeed;
            //_anim.SetBool("isNearFlashlight", true);
            Debug.Log("Hit by flashlight");
            _isLitUp = true;
        }
        else if (!_flashlight.GetIsCollidingWithEnemy() && _isLitUp)
        {
            _agent.speed = _defaultMoveSpeed;
            //_anim.SetBool("isNearFlashlight", false);
            Debug.Log("No longer hit by flashlight");
            _isLitUp = false;
        }
    }

    void LampBehavior()
    {
        if (_enemyController.GetNearLightSource() && !_isLitUpLamp)
        {
            _agent.speed = _lampMoveSpeed;
            _isLitUpLamp = true;
        }
        else if (!_enemyController.GetNearLightSource() && _isLitUpLamp)
        {
            _agent.speed = _defaultMoveSpeed;
            _isLitUpLamp = false;
        }
    }
}
