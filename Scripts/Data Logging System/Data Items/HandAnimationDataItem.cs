using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

/// <summary>
/// Specialized data item for handling hand animations.
/// </summary>
public  abstract class HandAnimationDataItem : DataItem<string> {

    HandAnimation rightHandAnimation;
    HandAnimation leftHandAnimation;

    protected Dictionary<int, string> AnimationHashToName;

    // Use this for initialization
    void Start()
    {
        StartCoroutine(Setup());
        AnimationHashToName = new Dictionary<int, string>();
        AnimationHashToName.Add(HandAnimation.HandAnimationHashes.GrabLarge, HandAnimation.HandAnimationHashes.GrabLargeString);
        AnimationHashToName.Add(HandAnimation.HandAnimationHashes.Idle, HandAnimation.HandAnimationHashes.IdleString);
        AnimationHashToName.Add(HandAnimation.HandAnimationHashes.Point, HandAnimation.HandAnimationHashes.PointString);
        AnimationHashToName.Add(HandAnimation.HandAnimationHashes.ThumbUp, HandAnimation.HandAnimationHashes.ThumbUpString);

    }

    public override void OnEnable()
    {
        base.OnEnable();
        
        if (rightHandAnimation != null)
        {
            rightHandAnimation.OnAnimationChange += OnAnimationChange;
            
        }

        if (leftHandAnimation != null)
        {
            leftHandAnimation.OnAnimationChange += OnAnimationChange;
        }
    }

    public override void OnDisable()
    {
        base.OnDisable();

        if (rightHandAnimation != null)
        {
            rightHandAnimation.OnAnimationChange -= OnAnimationChange;

        }

        if (leftHandAnimation != null)
        {
            leftHandAnimation.OnAnimationChange -= OnAnimationChange;
        }
    }

    IEnumerator Setup()
    {
        yield return new WaitUntil(() => Player.instance.rightHand != null && Player.instance.leftHand != null);
        rightHandAnimation = Player.instance.rightHand.GetComponentInChildren<HandAnimation>();
        leftHandAnimation = Player.instance.leftHand.GetComponentInChildren<HandAnimation>();
        rightHandAnimation.OnAnimationChange += OnAnimationChange;
        leftHandAnimation.OnAnimationChange += OnAnimationChange;

        if (!rightHandAnimation.DataLoggers.Contains(Dataset))
        {
            rightHandAnimation.DataLoggers.Add(Dataset);
        }
        if (!leftHandAnimation.DataLoggers.Contains(Dataset))
        {
            leftHandAnimation.DataLoggers.Add(Dataset);
        }

    }

    public virtual void OnAnimationChange(int hash, Hand hand) { }
}
