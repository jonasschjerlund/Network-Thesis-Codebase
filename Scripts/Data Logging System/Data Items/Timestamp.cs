using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Timestamp : DataItem<string> {

    public override void OnDataLoggingRequested()
    {
        value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff zzz");
    }
}
