using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// 
/// </summary>
public class BehaviorSeenByPlayer : IEnemyBehavior
{
    // Called attributes
    EnemyController _enemy;
    NavMeshAgent _agent;
    float _defaultEnemyMoveSpeed;
    float _seenByPlayerEnemyMoveSpeed;

    // Local attributes
    bool _isSeen;

    public BehaviorSeenByPlayer(AttributeStorage attributes)
    {
        _enemy = attributes.GetEnemyController();
        _defaultEnemyMoveSpeed = attributes.GetDefaultEnemyMoveSpeed();
        _seenByPlayerEnemyMoveSpeed = attributes.GetSeenByPlayerEnemyMoveSpeed();
        _agent = attributes.GetEnemyController().GetComponent<NavMeshAgent>();
    }

    public void Behavior()
    {
        if(_isSeen)
            _agent.speed = _seenByPlayerEnemyMoveSpeed;
        else if(!_isSeen)
            _agent.speed = _defaultEnemyMoveSpeed;
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
    }

    public bool CheckState()
    {
        return true;
    }

    public void DebugFunction()
    {
        Debug.Log("SEEN BY PLAYER behavior WORKS");
    }

    public void SetState(bool value)
    {
         
    }

    public string BehaviorMessage()
    {
        return "Moves faster if seen";
    }
}
