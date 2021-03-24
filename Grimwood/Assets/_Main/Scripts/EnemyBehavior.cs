using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyBehavior
{
    public string behaviorTheme;

    [SerializeField] int _childStatesCount;
    int _parentState; // Behavior that is chosen
    int[] _ChildState; // Behaviors to choose from

    static readonly System.Random rnd = new System.Random();

    [SerializeField] IBehavior[] behavior;

    public void GenerateBehaviorStates()
    {
        _ChildState = new int[_childStatesCount];
        for (int i = 0; i < _ChildState.Length; i++)
        {
            _ChildState[i] = i;
        }

        // Get a random Parent state for each children pair
        _parentState = rnd.Next(_childStatesCount);
    }

    public void GenerateBehaviorStates2()
    {

    }

    public int GetParentState()
    {
        return _parentState;
    }

    public void SetParentState(int value)
    {
        _parentState = value;
    }
}
