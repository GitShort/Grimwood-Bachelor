using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;


public class TestEnemyInterface : MonoBehaviour
{
    public BehaviorAttributes attributes;
    [SerializeField] EnemyController _enemyContr;
    public EnemyBehavior[] Behaviors;
 

    // Start is called before the first frame update
    void Start()
    {
        foreach (EnemyBehavior behavior in Behaviors)
        {
            //SelectBehaviors(behavior);
            behavior.GenerateBehaviorStates(attributes);
            _enemyContr.AddBehavior(behavior.GetBehaviorReference(), behavior.GetKey());
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //void SelectBehaviors(EnemyBehavior behavior)
    //{
    //    System.Random rnd = new System.Random();
    //    behavior.SetParentState(rnd.Next(behavior.GetChildStateCount()));
    //}
}
