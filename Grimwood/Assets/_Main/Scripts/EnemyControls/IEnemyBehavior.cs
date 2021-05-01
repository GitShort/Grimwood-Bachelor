using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemyBehavior
{
    void Behavior();

    void DebugFunction();

    void CallBehavior();

    string BehaviorMessage();
}
