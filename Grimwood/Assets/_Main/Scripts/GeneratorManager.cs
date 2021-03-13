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

    // Start is called before the first frame update
    void Start()
    {
        _hoverbutton = GetComponent<HoverButton>();
        _hoverbutton.onButtonDown.AddListener(OnButtonDown);
        _localLevelLightmapData = FindObjectOfType<LSS_FrontEnd>();
    }

    void OnButtonDown(Hand hand)
    {
        if (GameManager.IsGeneratorOn)
        {
            _localLevelLightmapData.Load("Generator_Off");
            GameManager.IsGeneratorOn = false;
            _lampMesh.material.SetColor("_EmissionColor", Color.red);
            //Debug.Log("clicked OFF");
        }
        else
        {
            _localLevelLightmapData.Load("Generator_On");
            GameManager.IsGeneratorOn = true;
            _lampMesh.material.SetColor("_EmissionColor", Color.green);
            //Debug.Log("clicked ON");
        }

    }
}
