using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Username : DataItem<string> {

    public override void OnDataLoggingRequested()
    {
        value = DataSystemController.Instance.Username;
    }
}
