using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Finds a logging factory singleton and createes or destroys objects. 
/// Useful for scenes that use event calls but only get a factory at runtime.
/// </summary>
public class LoggingEventHelper : MonoBehaviour {

    public string[] datasetsToLog;

    public void InvokeStart()
    {
        foreach(string name in datasetsToLog)
        {
            DatasetFactory.Instance.CreateDataset(name);
        }
    }

    public void InvokeStop()
    {
        foreach (string name in datasetsToLog)
        {
            DatasetFactory.Instance.StopDatasetLogging(name);
        }
    }
}
