using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviorDisappearDoor : IEnemyBehavior
{
    EnemyController _enemy;

    public BehaviorDisappearDoor(AttributeStorage attributes)
    {
        _enemy = attributes.GetEnemyController();
    }

    public void Behavior()
    {
        if (_enemy.GetNearDoor())
        {
            _enemy.SetShouldDisappear(true);
        }
    }

    public void CallBehavior()
    {

    }

    public void DebugFunction()
    {
        Debug.Log("Disappear behind door behavior WORKS");
    }

    public string BehaviorMessage()
    {
        return "Hide in a room";
    }
}
