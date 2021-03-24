using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TestEnemyInterface : MonoBehaviour
{
    public EnemyBehavior[] Behaviors;

    // Start is called before the first frame update
    void Start()
    {
        foreach (EnemyBehavior behavior in Behaviors)
        {
            //SelectBehaviors(behavior);
            behavior.GenerateBehaviorStates();
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
