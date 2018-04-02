using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Valve.VR.InteractionSystem;

/// <summary>
/// Contains functionality to move a game object smoothly through a series of positions.
/// </summary>
public class SmoothMotion : NetworkBehaviour {

    [SerializeField]
    private Transform[] TravelPath;

    public float SmoothTime = 1.0f;

    public bool StartMoving = false;

    private Rigidbody rb;
    private Throwable throwable;
    private bool isMoving;
    private Vector3[] positions;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>() ?? null;
        throwable = GetComponent<Throwable>() ?? null;

        positions = new Vector3[TravelPath.Length];

        for (int i = 0; i < TravelPath.Length; i++)
        {
            positions[i] = TravelPath[i].position;
        }
	}

    void Update()
    {
        if (StartMoving)
        {
            if (!isMoving) CmdMoveSmoothly(positions);
            StartMoving = false;
        }
    }

    [Command]
    public void CmdMoveSmoothly(Vector3[] travelPath)
    {
        RpcMoveSmoothly(travelPath);
    }

    [ClientRpc]
    void RpcMoveSmoothly(Vector3[] travelPath)
    {
        StartCoroutine(MoveSmoothly(travelPath));
    }

    /// <summary>
    /// Invokes a server command. Method signature meets Unity event callback requirements. 
    /// </summary>
    public void StartSmoothMove()
    {
        CmdMoveSmoothly(positions);
    }


    /// <summary>
    /// Moves a game object smoothly through a series of positions, one at a time.
    /// </summary>
    /// <param name="travelPath">Positions representing world space coordinates.</param>
    IEnumerator MoveSmoothly(Vector3[] travelPath)
    {
        isMoving = true;

        if (rb != null) rb.isKinematic = true;
        if (throwable != null) throwable.active = false;

        int i = 0;
        
        while (i < travelPath.Length)
        {
            Vector3 velocity = Vector3.zero;
            while (Vector3.Distance(transform.position, travelPath[i]) > 0.2f)
            {
                // Smooth damp defaults to delta time for time step, so we don't need to multiply with delta time here
                transform.position = Vector3.SmoothDamp(transform.position, travelPath[i], ref velocity, SmoothTime);
                yield return new WaitForEndOfFrame();
            }
            i++;
        }

        if (rb != null) rb.isKinematic = false;
        if (throwable != null) throwable.active = true;

        isMoving = false;
    }
}