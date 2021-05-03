using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;

namespace Tests
{
    public class EnemyTests
    {
        [SetUp]
        public void Init()
        {
#if UNITY_EDITOR
            UnityEditor.SceneManagement.EditorSceneManager.LoadScene("MainTest", LoadSceneMode.Single);
#endif
        }

        [UnityTest]
        public IEnumerator EnemyBehaviorManager()
        {
            // Assign
            GameObject go = new GameObject();
            go.SetActive(false);
            EnemyBehaviorManager findComponents = GameObject.FindObjectOfType<EnemyBehaviorManager>();
            EnemyBehaviorManager behaviorManager = go.AddComponent<EnemyBehaviorManager>();

            behaviorManager.Behaviors = new EnemyBehavior[5];
            behaviorManager.Behaviors[0] = new EnemyBehavior();
            behaviorManager.Behaviors[0]._ChildState = new UnityEngine.Object[2];
            behaviorManager.Behaviors[0]._ChildState[0] = new GameObject("BehaviorFreeze");
            behaviorManager.Behaviors[0]._ChildState[1] = new GameObject("BehaviorBlind");

            behaviorManager.Behaviors[1] = new EnemyBehavior();
            behaviorManager.Behaviors[1]._ChildState = new UnityEngine.Object[2];
            behaviorManager.Behaviors[1]._ChildState[0] = new GameObject("BehaviorCameraFlash");
            behaviorManager.Behaviors[1]._ChildState[1] = new GameObject("BehaviorDisappearDoor");

            behaviorManager.Behaviors[2] = new EnemyBehavior();
            behaviorManager.Behaviors[2]._ChildState = new UnityEngine.Object[2];
            behaviorManager.Behaviors[2]._ChildState[0] = new GameObject("BehaviorLightStrong");
            behaviorManager.Behaviors[2]._ChildState[1] = new GameObject("BehaviorLightWeak");

            behaviorManager.Behaviors[3] = new EnemyBehavior();
            behaviorManager.Behaviors[3]._ChildState = new UnityEngine.Object[2];
            behaviorManager.Behaviors[3]._ChildState[0] = new GameObject("BehaviorMoveWithoutSound");
            behaviorManager.Behaviors[3]._ChildState[1] = new GameObject("BehaviorMoveWithSound");

            behaviorManager.Behaviors[4] = new EnemyBehavior();
            behaviorManager.Behaviors[4]._ChildState = new UnityEngine.Object[3];
            behaviorManager.Behaviors[4]._ChildState[0] = new GameObject("BehaviorSeenByPlayer");
            behaviorManager.Behaviors[4]._ChildState[1] = new GameObject("BehaviorUnseenByPlayer");
            behaviorManager.Behaviors[4]._ChildState[2] = new GameObject("BehaviorDamageSeenPlayer");

            behaviorManager.attributes = findComponents.attributes;
            go.SetActive(true);
            // Act
            //enemyBehavior.GenerateBehaviorStates(attribute);



            yield return new WaitForFixedUpdate();

            // Assert
            for (int i = 0; i < behaviorManager.Behaviors.Length; i++)
            {
                Assert.IsNotNull(behaviorManager.Behaviors[i].GetBehaviorReference());
            }
        }
    }
}
