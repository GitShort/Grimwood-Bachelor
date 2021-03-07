using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class FlashlightManager : MonoBehaviour
{
    public SteamVR_Action_Boolean ActionToggle = SteamVR_Input.GetBooleanAction("TriggerToggle");

    private Interactable _interactable;

    [SerializeField] GameObject _lightObjs;

    // Start is called before the first frame update
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
                //Debug.Log("Flashlight toggle " + hand);
                if (_lightObjs.activeInHierarchy)
                    _lightObjs.SetActive(false);
                else
                    _lightObjs.SetActive(true);
            }
        }
    }
}
