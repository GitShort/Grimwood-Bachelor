using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;
using LSS;

public class GeneratorManager : MonoBehaviour
{
    [SerializeField] HoverButton _hoverButton;
    private LSS_FrontEnd _localLevelLightmapData;
    [SerializeField] MeshRenderer _lampMesh;

    bool IsEnemyHittingGenerator = false;

    void Start()
    {
        _hoverButton.onButtonDown.AddListener(OnButtonDown);
        _localLevelLightmapData = FindObjectOfType<LSS_FrontEnd>();
    }

    private void Update()
    {
        if (IsEnemyHittingGenerator)
        {
            GeneratorOff();
        }

        //TEST
        GeneratorDebugging();
    }

    void OnButtonDown(Hand hand)
    {
        if (GameManager.GetIsGeneratorOn())
        {
            GeneratorOff();
        }
        else
        {
            GeneratorOn();
        }

    }

    void GeneratorOff()
    {
        _localLevelLightmapData.Load("Generator_Off");
        GameManager.SetIsGeneratorOn(false);
        _lampMesh.material.SetColor("_EmissionColor", Color.red);
        //Debug.Log("clicked OFF");
        IsEnemyHittingGenerator = false;
    }

    void GeneratorOn()
    {
        _localLevelLightmapData.Load("Generator_On");
        GameManager.SetIsGeneratorOn(true);
        _lampMesh.material.SetColor("_EmissionColor", Color.green);
        //Debug.Log("clicked ON");
    }

    public bool GetIsEnemyHittingGenerator()
    {
        return IsEnemyHittingGenerator;
    }

    public void SetIsEnemyHittingGenerator(bool value)
    {
        IsEnemyHittingGenerator = value;
    }

    /// <summary>
    /// FOR TESTING PURPOSES
    /// </summary>
    public void GeneratorDebugging()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            GeneratorOn();
        }
        else if (Input.GetKey(KeyCode.Alpha2))
        {
            GeneratorOff();
        }
    }
}
