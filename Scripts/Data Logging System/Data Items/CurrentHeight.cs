using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

/// <summary>
/// Logs the current height of the player. 
/// If the HMD is unassigned, will try to find an instance of the Player
/// component from SteamVR's interaction system. See also: 
/// <seealso cref="Player"/>
/// </summary>
public class CurrentHeight : DataItem<float> {

    private Transform HMD;

    void Start()
    {
        if (Player.instance == null)
        {
            Debug.LogError("Could not find HMD transform. Height is not being logged.");
            return;
        }
        HMD = Player.instance.hmdTransform;

        value = HMD.localPosition.y;
    }

    public override void OnDataLoggingRequested()
    {
        if (HMD != null)
        {
            value = HMD.localPosition.y;
        }
    }
}