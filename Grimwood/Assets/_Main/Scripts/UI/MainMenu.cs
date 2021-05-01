using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering;

public class MainMenu : MonoBehaviour
{
    [SerializeField] RenderPipelineAsset[] _qualityLevels;

    public void StartGame()
    {
        Debug.Log("Game started");
        SceneManager.LoadScene("Main", LoadSceneMode.Single);
    }

    public void ExitGame()
    {
        Debug.Log("EXIT");
        Application.Quit();
    }

    public void SetQualityLevel(int value)
    {
        Debug.Log("Changed Quality level " + value);
        QualitySettings.SetQualityLevel(value);
        QualitySettings.renderPipeline = _qualityLevels[value];
    }


}
