using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class Gesture : HandAnimationDataItem {

    public override void OnAnimationChange(int hash, Hand hand)
    {
        value = AnimationHashToName[hash];
    }
}
