using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestingLinecast : MonoBehaviour
{
    [SerializeField] Transform _player;
    [SerializeField] LayerMask _ignoredLayers;
    [SerializeField] Renderer _renderer;
    RaycastHit hit;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        IsEnemyVisible();
    }
    void IsEnemyVisible()
    {
        //Debug.Log(Physics.Linecast(transform.position, _player.position, out hit, _ignoredLayers));
        if (Physics.Linecast(transform.position, _player.position, out hit, _ignoredLayers, QueryTriggerInteraction.Ignore))
        {
            Debug.Log(hit.collider.name);
            Debug.DrawLine(transform.position, _player.position);

            if (_renderer.isVisible && hit.collider.gameObject.CompareTag("Player"))
                Debug.Log("Visible");
            else
                Debug.Log("Hidden");
        }


    }
}
