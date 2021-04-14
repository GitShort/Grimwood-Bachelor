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
    GeneratorManager _energyGen;

    // Local attributes
    bool _isLitUp = false;
    bool _isLitUpLamp = false;

    bool _sentGeneratorSignal = false;

    static readonly System.Random rnd = new System.Random();
    float _chanceToFollowGenerator;
    int _followGeneratorValue = 10;
    bool _randomValueGenerated = false;

    bool _check = false;

    public BehaviorLightWeak(AttributeStorage attributes)
    {
        _enemyController = attributes.GetEnemyController();
        _flashlightMoveSpeed = attributes.GetFlashlightEnemyMoveSpeedWeak();
        _lampMoveSpeed = attributes.GetLampEnemyMoveSpeedWeak();
        _defaultMoveSpeed = attributes.GetDefaultEnemyMoveSpeed();
        _anim = attributes.GetEnemyController().GetComponent<Animator>();
        _agent = attributes.GetEnemyController().GetComponent<NavMeshAgent>();
        _flashlight = attributes.GetFlashlightManager();
        _energyGen = attributes.GetGeneratorManager();
    }

    public void Behavior()
    {
        FlashlightBehavior();
        LampBehavior();
    }

    public void CallBehavior()
    {
        // used to check if a random chance to change target from player to generator is selected
        if (GameManager.instance.GetIsGeneratorOn() && !_randomValueGenerated && !_enemyController.GetShouldDisappear())
        {
            _chanceToFollowGenerator = rnd.Next(_followGeneratorValue);
            _randomValueGenerated = true;
            Debug.Log(_chanceToFollowGenerator);
        }
        else if (_randomValueGenerated && _enemyController.GetShouldDisappear())
        {
            _randomValueGenerated = false;
            Debug.Log("Removed");
        }


        if (GameManager.instance.GetIsGeneratorOn() && !_sentGeneratorSignal && _chanceToFollowGenerator < _followGeneratorValue - 5 && _randomValueGenerated)
        {
            _sentGeneratorSignal = true;
            _enemyController.SetGoToGenerator(true);
        }
        else if ((_chanceToFollowGenerator > _followGeneratorValue - 5 && _sentGeneratorSignal) || (!GameManager.instance.GetIsGeneratorOn() && _sentGeneratorSignal))
        {
            _sentGeneratorSignal = false;
            _enemyController.SetGoToGenerator(false);
            Debug.Log("Reached");
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

    public string BehaviorMessage()
    {
        return "Afraid of light";
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
