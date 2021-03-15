using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LSS;

public class GameManager : MonoBehaviour
{
    public static bool IsGeneratorOn;
    public static bool IsMonsterHittingGenerator;

    // Start is called before the first frame update
    void Start()
    {
        IsGeneratorOn = false;
        IsMonsterHittingGenerator = false;
    }

    // Update is called once per frame
    void Update()
    {
    }
}
