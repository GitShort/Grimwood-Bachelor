using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BehaviorLightStrong : IEnemyBehavior
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

    public BehaviorLightStrong(AttributeStorage attributes)
    {
        _enemyController = attributes.GetEnemyController();
        _flashlightMoveSpeed = attributes.GetFlashlightEnemyMoveSpeedStrong();
        _lampMoveSpeed = attributes.GetLampEnemyMoveSpeedStrong();
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

    }

    public void DebugFunction()
    {
        Debug.Log("FLASHLIGHT strong behavior WORKS");
    }

    public string BehaviorMessage()
    {
        return "Light makes it stronger";
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
