using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectListing : MonoBehaviour
{
    public DynamicList<Effect> myEffects = new DynamicList<Effect>();

    [ContextMenu("Trigger")]
    public void TriggerEffects()
    {
        foreach(Effect effect in myEffects)
        {
            effect.Trigger();
        }
    }

}
