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

    bool _isEnemyHittingGenerator = false;
    bool _generatorhit = false;

    void Start()
    {
        _hoverButton.onButtonDown.AddListener(OnButtonDown);
        _localLevelLightmapData = FindObjectOfType<LSS_FrontEnd>();
        _localLevelLightmapData.Load("Generator_Off");
    }

    private void Update()
    {
        if (_isEnemyHittingGenerator && !_generatorhit)
        {
            _generatorhit = true;
            AudioManager.instance.Play("GeneratorOffEnemy", this.gameObject);
            GeneratorOff();
        }

        //TEST
        GeneratorDebugging();
    }

    void OnButtonDown(Hand hand)
    {
        if (GameManager.instance.GetIsGeneratorOn())
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
        if (!_isEnemyHittingGenerator)
            AudioManager.instance.Play("Generator", this.gameObject);

        _localLevelLightmapData.Load("Generator_Off");
        GameManager.instance.SetIsGeneratorOn(false);
        _lampMesh.material.SetColor("_EmissionColor", Color.red);
        //Debug.Log("clicked OFF");
        _isEnemyHittingGenerator = false;
    }

    void GeneratorOn()
    {
        AudioManager.instance.Play("Generator", this.gameObject);
        _localLevelLightmapData.Load("Generator_On");
        GameManager.instance.SetIsGeneratorOn(true);
        _lampMesh.material.SetColor("_EmissionColor", Color.green);
        //Debug.Log("clicked ON");
    }

    public bool GetIsEnemyHittingGenerator()
    {
        return _isEnemyHittingGenerator;
    }

    public void SetIsEnemyHittingGenerator(bool value)
    {
        _isEnemyHittingGenerator = value;
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
