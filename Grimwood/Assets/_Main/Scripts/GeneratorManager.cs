using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;
using LSS;

public class GeneratorManager : MonoBehaviour
{
    HoverButton _hoverbutton;
    private LSS_FrontEnd _localLevelLightmapData;
    [SerializeField] MeshRenderer _lampMesh;

    void Start()
    {
        _hoverbutton = GetComponent<HoverButton>();
        _hoverbutton.onButtonDown.AddListener(OnButtonDown);
        _localLevelLightmapData = FindObjectOfType<LSS_FrontEnd>();
    }

    private void Update()
    {
        if (GameManager.IsMonsterHittingGenerator)
        {
            GeneratorOff();
        }
    }

    void OnButtonDown(Hand hand)
    {
        if (GameManager.IsGeneratorOn)
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
        GameManager.IsGeneratorOn = false;
        _lampMesh.material.SetColor("_EmissionColor", Color.red);
        //Debug.Log("clicked OFF");
        GameManager.IsMonsterHittingGenerator = false;
    }

    void GeneratorOn()
    {
        _localLevelLightmapData.Load("Generator_On");
        GameManager.IsGeneratorOn = true;
        _lampMesh.material.SetColor("_EmissionColor", Color.green);
        //Debug.Log("clicked ON");
    }
}
