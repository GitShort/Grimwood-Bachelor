using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LSS;

public class GameManager : MonoBehaviour
{
    bool _isGeneratorOn;

    public static GameManager instance;

    bool _isPlayerAlive;
    int collectiblesCount; // think of a new name for collectibe hints

    private void Awake()
    {
        if (instance == null)
            instance = this;

        RenderSettings.fog = true;
    }

    void Start()
    {
        collectiblesCount = 0;
        _isGeneratorOn = false;
        _isPlayerAlive = false;
        AudioManager.instance.Play("Environment", this.gameObject);
    }

    void Update()
    {
        
    }

    public bool GetIsGeneratorOn()
    {
        return _isGeneratorOn;
    }

    public void SetIsGeneratorOn(bool value)
    {
        _isGeneratorOn = value;
    }

    public bool GetIsPlayerAlive()
    {
        return _isPlayerAlive;
    }

    public void SetIsPlayerAlive(bool value)
    {
        _isPlayerAlive = value;
    }
}
