using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemyBehavior
{
    void Behavior();

    bool CheckState();
    void SetState(bool value);

    void DebugFunction();

    void CallBehavior();
}
