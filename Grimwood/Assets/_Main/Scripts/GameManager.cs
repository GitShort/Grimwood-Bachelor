using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LSS;

public class GameManager : MonoBehaviour
{
    static bool IsGeneratorOn;

    void Start()
    {
        IsGeneratorOn = false;
    }

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
