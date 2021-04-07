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
    bool _isNearGenerator = false;
    bool _isLitUpLamp = false;

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

    // checks if monster is near the energy generator
    //void EnergyGeneratorAction()
    //{
    //    if (Physics.Linecast(castsPosition, _energyGenPos.position, out hit, _includedLayers, QueryTriggerInteraction.Ignore))
    //    {
    //        //Debug.Log(hit.collider.name);
    //        //Debug.DrawLine(castsPosition, _player.position);

    //        if (hit.collider.gameObject.CompareTag("Generator"))
    //        {
    //            _energyGen = hit.collider.GetComponent<GeneratorManager>();
    //            //hit.collider.gameObject.GetComponent<GeneratorManager>();
    //            Invoke("HitEnergyGenerator", 1.5f);
    //        }
    //    }
    //}

    //void HitEnergyGenerator()
    //{
    //    _energyGen.SetIsEnemyHittingGenerator(true);
    //    Debug.Log("Monster has hit the generator!");
    //}
}
