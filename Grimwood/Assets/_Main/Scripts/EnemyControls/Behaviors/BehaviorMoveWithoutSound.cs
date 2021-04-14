using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BehaviorMoveWithoutSound : IEnemyBehavior
{
    // Called attributes
    EnemyController _enemy;
    NavMeshAgent _agent;

    // Local attributes

    bool _didBreath = false;
    float _breathTimer = 0f;
    bool _isbreathTimerStarted;
    bool _isbreathTimerFinished = false;
    float _breathSoundInterval = 3f;

    static readonly System.Random rnd = new System.Random();
    float _generatedChoice = 0;

    public BehaviorMoveWithoutSound(AttributeStorage attributes)
    {
        _agent = attributes.GetEnemyController().GetComponent<NavMeshAgent>();
        _enemy = attributes.GetEnemyController();
    }

    public void Behavior()
    {
        if (!_isbreathTimerStarted && !_isbreathTimerFinished)
        {
            _breathTimer = 0f;
            _isbreathTimerStarted = true;
        }
        if (_isbreathTimerStarted && !_isbreathTimerFinished)
        {
            _breathTimer += Time.deltaTime;
            if (_breathSoundInterval <= _breathTimer && !_didBreath)
            {
                _didBreath = true;
                _isbreathTimerFinished = false;
                _isbreathTimerStarted = false;
            }
        }
        if (_didBreath)
        {
            _generatedChoice = rnd.Next(10);
            //Debug.Log(_generatedChoice);


            if (_generatedChoice >= 8)
            {
                AudioManager.instance.Play("MonsterGrowl", _enemy.gameObject);
            }
            else
                AudioManager.instance.Play("MonsterBreath", _enemy.gameObject);

            _didBreath = false;
        }
    }

    public void CallBehavior()
    {

    }

    public bool CheckState()
    {
        return true;
    }

    public void DebugFunction()
    {
        Debug.Log("BehaviorMoveWithoutSound");
    }

    public void SetState(bool value)
    {

    }

    public string BehaviorMessage()
    {
        return "Moves silently";
    }
}
