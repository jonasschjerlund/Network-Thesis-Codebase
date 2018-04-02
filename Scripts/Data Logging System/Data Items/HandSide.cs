using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class HandSide : HandAnimationDataItem {

    public override void OnAnimationChange(int hash, Hand hand)
    {
        if (hand.GuessCurrentHandType() == Hand.HandType.Left)
        {
            value = "Left";
        }
        else if (hand.GuessCurrentHandType() == Hand.HandType.Right)
        {
            value = "Right";
        }
    }
}
