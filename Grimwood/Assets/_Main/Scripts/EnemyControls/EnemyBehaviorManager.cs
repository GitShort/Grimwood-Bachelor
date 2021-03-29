using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;


public class EnemyBehaviorManager : MonoBehaviour
{
    public AttributeStorage attributes;
    [SerializeField] EnemyController _enemyContr;
    public EnemyBehavior[] Behaviors;
 
    void Start()
    {
        foreach (EnemyBehavior behavior in Behaviors)
        {
            //SelectBehaviors(behavior);
            behavior.GenerateBehaviorStates(attributes);
            _enemyContr.AddBehavior(behavior.GetBehaviorReference(), behavior.GetKey());
        }

    }

}
