using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class TeleportationCounter : SingletonMonoBehaviour<TeleportationCounter> {

    public int TotalTeleports;
    
    // Use this for initialization
    void Start ()
    {
        SubscribeToEvents();
    }

    void OnEnable()
    {
        SubscribeToEvents();
    }

    void OnDisable()
    {
        UnsubscribeFromEvents();
    }

    void SubscribeToEvents()
    {
        if (Teleport.instance != null)
        {
            Teleport.instance.OnTeleport += OnTeleport;
        }
    }

    void UnsubscribeFromEvents()
    {
        if (Teleport.instance != null)
        {
            Teleport.instance.OnTeleport -= OnTeleport;
        }
    }
    
    private void OnTeleport()
    {
        TotalTeleports++;
    }

    public void Reset()
    {
        TotalTeleports = 0;
    }
}