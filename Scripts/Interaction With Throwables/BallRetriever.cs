using StixGames.GrassShader;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class BallRetriever : MonoBehaviour {

    public float SmoothTime = 0.5f;

    public Transform Ball;

    Hand hand;


    Rigidbody ballRigidbody;
    DisplacementTrailRenderer ballDisplacementTrailRenderer;

    Vector3 referencePosition;
    bool running;

	// Use this for initialization
	void Start () {
        hand = GetComponent<Hand>() ?? null;

        ballRigidbody = Ball.gameObject.GetComponent<Rigidbody>();
        ballDisplacementTrailRenderer = Ball.gameObject.GetComponent<DisplacementTrailRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
        if (hand.controller.GetPressDown(SteamVR_Controller.ButtonMask.ApplicationMenu))
        {
            referencePosition = hand.transform.position;
            StartCoroutine(SummonBall(referencePosition));
        }
	}


    IEnumerator SummonBall(Vector3 destination) 
    {
        if (running) yield return null;

        running = true;

        ballRigidbody.isKinematic = true;
        ballDisplacementTrailRenderer.createTrail = false;

        Vector3 velocity = Vector3.zero;
        while (Vector3.Distance(Ball.position, destination) > 0.2f)
        {
            // Smooth damp defaults to delta time for time step, so we don't need to multiply with delta time here
            Ball.position = Vector3.SmoothDamp(Ball.position, destination, ref velocity, SmoothTime);
            yield return new WaitForEndOfFrame();
        }

        ballRigidbody.isKinematic = false;
        ballDisplacementTrailRenderer.createTrail = true;
        running = false;
    }
}
