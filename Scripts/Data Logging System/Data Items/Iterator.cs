using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///  Simple iterator that starts at a given value and increments by
///  1 each time data is logged.
/// </summary>
public class Iterator : DataItem<int> {

    /// <summary>
    /// Which index should the iterator start at?
    /// </summary>
    [Tooltip("Which index should the iterator start at?")]
    public int StartAt = 1;

    void Start()
    {
        value = StartAt-1;
    }

    public override void OnDataLoggingRequested()
    {
        value++;
    }
}
