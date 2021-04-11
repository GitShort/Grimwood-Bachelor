using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BehaviorMoveWithSound : IEnemyBehavior
{
    // Called attributes
    EnemyController _enemy;
    NavMeshAgent _agent;

    public BehaviorMoveWithSound(AttributeStorage attributes)
    {
        _agent = attributes.GetEnemyController().GetComponent<NavMeshAgent>();
        _enemy = attributes.GetEnemyController();
    }

    public void Behavior()
    {
        
    }

    public void CallBehavior()
    {
        if (_agent.velocity != Vector3.zero)
        {
            AudioManager.instance.Play("EnemySteps", _enemy.gameObject);
            //Debug.Log("Steps playing");
        }
        else
        {
            AudioManager.instance.Stop("EnemySteps", _enemy.gameObject);
        }
    }

    public bool CheckState()
    {
        return true;
    }

    public void DebugFunction()
    {
        
    }

    public void SetState(bool value)
    {
        
    }
}
