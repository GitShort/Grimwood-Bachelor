using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

[System.Serializable]
public class EnemyBehavior
{
    public IEnemyBehavior _behaviorReference;

    int _behaviorIndex; // Behavior that is chosen
    public UnityEngine.Object[] _ChildState;

    static readonly System.Random rnd = new System.Random();
    string key;

    public void GenerateBehaviorStates(AttributeStorage attributes)
    {
        
        // Get a random Parent state for each children pair
        _behaviorIndex = rnd.Next(_ChildState.Length);
        
        _behaviorReference = (IEnemyBehavior)GetInstance(_ChildState[_behaviorIndex].name, attributes);
    }

    public IEnemyBehavior GetBehaviorReference()
    {
        return _behaviorReference;
    }

    public string GetKey()
    {
        return key;
    }

    // class name is key
    public object GetInstance(string strFullyQualifiedName, AttributeStorage attributes)
    {
        Type t = Type.GetType(strFullyQualifiedName);
        System.Object[] args = { attributes };
        key = strFullyQualifiedName;
        return Activator.CreateInstance(t, args);
    } 
}
