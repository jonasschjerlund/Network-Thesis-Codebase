using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The total amount of teleports so far.
/// </summary>
public class TotalTeleports : DataItem<int> {

    public override void OnDataLoggingRequested()
    {
        value = TeleportationCounter.Instance.TotalTeleports;
    }
}
