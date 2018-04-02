using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

/// <summary>
/// Adds and removes game object from the singleton object reference component's list of throwables.
/// </summary>
[RequireComponent(typeof(Throwable))]
public class AddSelfToThrowables : MonoBehaviour {

    void OnEnable()
    {
        MazeKeyObjectsReference.Instance.Throwables.Add(GetComponent<Throwable>());
        MazeKeyObjectsReference.Instance.AuthorityHandlers.Add(GetComponent<AuthorityHandler>());
    }

    void OnDisable()
    {
        // OnDisable gets called haphazardly on application exit, so check for singleton existence
        if (MazeKeyObjectsReference.Instance != null)
        {
            MazeKeyObjectsReference.Instance.Throwables.Remove(GetComponent<Throwable>());
            MazeKeyObjectsReference.Instance.AuthorityHandlers.Remove(GetComponent<AuthorityHandler>());
        }
    }
}
