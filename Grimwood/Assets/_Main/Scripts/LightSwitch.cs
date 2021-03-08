using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve;
using Valve.VR.InteractionSystem;

public class LightSwitch : MonoBehaviour
{
    public void OnButtonDown(Hand fromHand)
    {
        fromHand.TriggerHapticPulse(500);
    }

    public void OnButtonUp(Hand fromHand)
    {
        
    }


}
