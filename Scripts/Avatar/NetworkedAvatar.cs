using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Valve.VR.InteractionSystem;

public class NetworkedAvatar : NetworkBehaviour {

    public Transform HMDRepresentation;
    public Transform LeftControllerRepresentation;
    public Transform RightControllerRepresentation;

    public Hand leftController { get; private set; }
    public Hand rightController { get; private set; }

    private Transform HMD;

    private Transform leftControllerTransform;
    private Transform rightControllerTransform;


    // Unity method that gets called only for the local client
    public override void OnStartLocalPlayer()
    {
        gameObject.tag = "LocalAvatar";

        HMD = Player.instance.hmdTransform;
        leftController = Player.instance.hands[1];
        rightController = Player.instance.hands[0];
        leftControllerTransform = leftController.transform;
        rightControllerTransform = rightController.transform;

        // Locally disable renderers for your own avatar representation
        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers)
        {
            renderer.enabled = false;
        }
    }

    // Update is called once per frame
    void Update () {
        if (isLocalPlayer)
        {
            HMDRepresentation.position = HMD.position;
            HMDRepresentation.rotation = HMD.rotation;
            LeftControllerRepresentation.position = leftControllerTransform.position;
            LeftControllerRepresentation.rotation = leftControllerTransform.rotation;
            RightControllerRepresentation.position = rightControllerTransform.position;
            RightControllerRepresentation.rotation = rightControllerTransform.rotation;

            Player.instance.transform.localScale = transform.localScale;
        }
	}

    public Hand GetHand(GameObject controllerRepresentation)
    {
        if (controllerRepresentation.transform == LeftControllerRepresentation)
        {
            return leftController;
        }
        else if (controllerRepresentation.transform == RightControllerRepresentation)
        {
            return rightController;
        }
        else
        {
            Debug.LogError("Tried to retrieve hand with invalid parameter");
            return null;
        }
    }

    public Hand GetHand(Transform controllerRepresentation)
    {
        if (controllerRepresentation == LeftControllerRepresentation)
        {
            return leftController;
        }
        else if (controllerRepresentation == RightControllerRepresentation)
        {
            return rightController;
        }
        else
        {
            Debug.LogError("Tried to retrieve hand with invalid parameter");
            return null;
        }
    }
}