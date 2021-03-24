using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LSS;

public class GameManager : MonoBehaviour
{
    bool IsGeneratorOn;

    public static GameManager instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    void Start()
    {
        IsGeneratorOn = false;
    }

    void Update()
    {
        
    }

    public bool GetIsGeneratorOn()
    {
        return IsGeneratorOn;
    }

    public void SetIsGeneratorOn(bool value)
    {
        IsGeneratorOn = value;
    }
}
