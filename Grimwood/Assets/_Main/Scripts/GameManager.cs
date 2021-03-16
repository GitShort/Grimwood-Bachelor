using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LSS;

public class GameManager : MonoBehaviour
{
    static bool IsGeneratorOn;


    // Start is called before the first frame update
    void Start()
    {
        IsGeneratorOn = false;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public static bool GetIsGeneratorOn()
    {
        return IsGeneratorOn;
    }

    public static void SetIsGeneratorOn(bool value)
    {
        IsGeneratorOn = value;
    }
}
