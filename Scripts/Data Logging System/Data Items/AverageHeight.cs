using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Valve.VR.InteractionSystem;

/// <summary>
/// Logs the average height of the player, synchronized with a fixed interval.
/// Will try to find an instance of the Player component from SteamVR's interaction system. 
/// See also: <seealso cref="Player"/>
/// </summary>
public class AverageHeight : DataItem<float> {

    /// <summary>
    /// How often should the system collect data (in seconds)?
    /// </summary>
    [Tooltip("How often should the system collect data (in seconds)?")]
    [Range(0f, 5f)]
    public float Interval = 1.0f;

    private Transform HMD;
    private List<float> HMDHeightValues;

    // Use this for initialization
    void Start () {
        if (Player.instance == null)
        {
            Debug.LogError("Could not find HMD transform. Height is not being logged.");
            return;
        }
        HMD = Player.instance.hmdTransform;

        HMDHeightValues = new List<float>();
        StartCoroutine(LogHeight(Interval));
    }

    public override void OnDataLoggingRequested()
    {
        if (HMDHeightValues != null)
        {
            value = HMDHeightValues.Average();
        }
    }

    IEnumerator LogHeight(float interval)
    {
        while (true)
        {
            HMDHeightValues.Add(HMD.localPosition.y);
            yield return new WaitForSeconds(interval);
        }
    }

}