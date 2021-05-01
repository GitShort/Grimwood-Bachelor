using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BehaviorUnseenByPlayer : IEnemyBehavior
{
    // Called attributes
    EnemyController _enemy;
    NavMeshAgent _agent;
    float _defaultEnemyMoveSpeed;
    float _unseenByPlayerEnemyMoveSpeed;

    // Local attributes
    bool _isSeen;

    public BehaviorUnseenByPlayer(AttributeStorage attributes)
    {
        _enemy = attributes.GetEnemyController();
        _defaultEnemyMoveSpeed = attributes.GetDefaultEnemyMoveSpeed();
        _unseenByPlayerEnemyMoveSpeed = attributes.GetUnseenByPlayerEnemyMoveSpeed();
        _agent = attributes.GetEnemyController().GetComponent<NavMeshAgent>();
    }

    public void Behavior()
    {
        if(_isSeen)
            _agent.speed = _defaultEnemyMoveSpeed;
        else if (!_isSeen)
            _agent.speed = _unseenByPlayerEnemyMoveSpeed;
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

    public void DebugFunction()
    {
        Debug.Log("UNSEEN BY PLAYER behavior WORKS");
    }

    public string BehaviorMessage()
    {
        return "Moves slower if seen";
    }
}
