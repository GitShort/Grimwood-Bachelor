using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshBuilder : MonoBehaviour
{
    NavMeshSurface _surface;

    // Start is called before the first frame update
    void Start()
    {
        _surface = GetComponent<NavMeshSurface>();

    }

    // Update is called once per frame
    void Update()
    {
        //_surface.UpdateNavMesh(_surface.navMeshData);
    }
}
