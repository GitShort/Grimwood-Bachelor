using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    bool _gameStarted = false;

    public void StartGame()
    {
        //for (int handIndex = 0; handIndex < Player.instance.hands.Length; handIndex++)
        //{
        //    Hand hand = Player.instance.hands[handIndex];
        //    if (hand != null && !_gameStarted)
        //    {
        //        Debug.Log("Game started");
        //        _gameStarted = true;
        //    }
        //}
        Debug.Log("Game started");
        SceneManager.LoadScene("Main", LoadSceneMode.Single);
    }

    public void ExitGame()
    {
        //for (int handIndex = 0; handIndex < Player.instance.hands.Length; handIndex++)
        //{
        //    Hand hand = Player.instance.hands[handIndex];
        //    if (hand != null)
        //    {
        //        Debug.Log("EXIT");
        //        Application.Quit();
        //    }
        //}
        Debug.Log("EXIT");
        Application.Quit();
    }


}
