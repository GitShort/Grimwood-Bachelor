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

    Dictionary<string, IEnemyBehavior> enemyBehaviors;

    private void Awake()
    {
        enemyBehaviors = new Dictionary<string, IEnemyBehavior>();
    }

    void Start()
    {
        foreach (EnemyBehavior behavior in Behaviors)
        {
            //SelectBehaviors(behavior);
            behavior.GenerateBehaviorStates(attributes);
            AddBehavior(behavior.GetBehaviorReference(), behavior.GetKey());
        }

    }

    private void Update()
    {
        foreach (string key in enemyBehaviors.Keys)
        {
            enemyBehaviors[key].CallBehavior();
            enemyBehaviors[key].Behavior();
            GameManager.instance.AddArtifactNames(enemyBehaviors[key].BehaviorMessage());
        }
    }

    public void AddBehavior(IEnemyBehavior behavior, string key)
    {
        enemyBehaviors.Add(key, behavior);
        behavior.DebugFunction();
    }

}
