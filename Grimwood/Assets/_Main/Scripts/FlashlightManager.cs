using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;
using VLB;

public class FlashlightManager : MonoBehaviour
{
    public SteamVR_Action_Boolean ActionToggle = SteamVR_Input.GetBooleanAction("TriggerToggle");

    Interactable _interactable;
    EnemyController _enemy;

    [SerializeField] GameObject _lightObjs;
    [SerializeField] float _lightRange = 6f;
    [SerializeField] VolumetricLightBeam _lightBeam;
    [SerializeField] LayerMask _layerMask;
    RaycastHit hit;
    Ray ray;

    void Start()
    {
        _interactable = GetComponent<Interactable>();
        _lightObjs.SetActive(false);
    }

    void Update()
    {
        if (_interactable.attachedToHand)
        {
            SteamVR_Input_Sources hand = _interactable.attachedToHand.handType;

            if (ActionToggle.GetStateDown(hand))
            {
                if (_lightObjs.activeInHierarchy)
                    _lightObjs.SetActive(false);
                else
                    _lightObjs.SetActive(true);
            }
        }
        if (_lightObjs.activeInHierarchy)
        {
            ray = new Ray(transform.position, transform.forward);
            if (Physics.Raycast(ray, out hit, _lightRange, _layerMask))
            {
                _lightBeam.fallOffEnd = hit.distance;
                _lightBeam.UpdateAfterManualPropertyChange();

                if (hit.collider.gameObject.CompareTag("Enemy"))
                {
                    _enemy = hit.collider.GetComponent<EnemyController>();
                    _enemy.SetIsCollidingWithFlashlight(true); // perdaryti i interface 
                }
                else if (!hit.collider.gameObject.CompareTag("Enemy") && _enemy != null)
                {
                    _enemy.SetIsCollidingWithFlashlight(false);
                }
            }
            else if (_enemy != null && _enemy.GetIsCollidingWithFlashlight())
                _enemy.SetIsCollidingWithFlashlight(false);
        }
        else if (_enemy != null)
            _enemy.SetIsCollidingWithFlashlight(false);
    }
}
