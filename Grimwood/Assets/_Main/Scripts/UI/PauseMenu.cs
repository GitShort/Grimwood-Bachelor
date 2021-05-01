using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;
using UnityEngine.SceneManagement;
using TMPro;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] Transform _playerHead;
    [SerializeField] GameObject _pauseMenu;
    [SerializeField] GameObject _loseMenu;
    [SerializeField] GameObject _winMenu;

    [SerializeField] TextMeshProUGUI[] _collectiblesText;

    [SerializeField] GameObject[] _PauseMenuItems;
    bool _pauseMenuReset = false;

    void Start()
    {
        _pauseMenu.gameObject.SetActive(false);
        _loseMenu.SetActive(false);
        _winMenu.SetActive(false);
    }

    private void Update()
    {
        if (!_pauseMenu.gameObject.activeInHierarchy && !_loseMenu.gameObject.activeInHierarchy && !_winMenu.gameObject.activeInHierarchy)
        {
            this.gameObject.transform.position = new Vector3(this.gameObject.transform.position.x, _playerHead.transform.position.y, this.gameObject.transform.position.z);
        }
        else
            _pauseMenuReset = false;
        if (!GameManager.instance.GetIsPlayerAlive())
            _loseMenu.SetActive(true);
        if (GameManager.instance.GetIsFinished())
            _winMenu.SetActive(true);


        Collection();
        if(!GameManager.instance.GetIsPaused() && !_pauseMenuReset)
            ResetMenu();
    }

    void ResetMenu()
    {
        foreach (GameObject item in _PauseMenuItems)
        {
            item.SetActive(false);
        }
        _PauseMenuItems[0].SetActive(true);
        _pauseMenuReset = true;

    }

    public void ResumeGame()
    {
        Debug.Log("Resumed");
        GameManager.instance.SetIsPaused(false);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("Main", LoadSceneMode.Single);
    }

    public void ExitGame()
    {
        SceneManager.LoadScene("Main Menu", LoadSceneMode.Single);
        Debug.Log("EXIT");
    }

    void Collection()
    {
        int count = 0;
        foreach (string item in GameManager.instance.GetCollectedArtifacts())
        {
            if (!GameManager.instance.GetCollectedArtifacts().Contains(_collectiblesText[count].text) && _collectiblesText[count].text.Equals("..."))
            {
                _collectiblesText[count].text = item;
            }
            count++;
        }
    }
}
