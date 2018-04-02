using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// Small server component built for resetting the position of the basketball
/// (but can be used for any object)
/// </summary>
public class EmergencyBallReset : NetworkBehaviour {

    /// <summary>
    /// Push this button to toggle the GUI button's visibility.
    /// </summary>
    [Tooltip("Push this button to toggle the GUI button's visibility.")]
    public KeyCode ToggleKey = KeyCode.T;

    /// <summary>
    /// Show the GUI button?
    /// </summary>
    [Tooltip("Show the GUI button?")]
    public bool ShowGUI = true;

    /// <summary>
    /// GUI X-axis offset for the button (from the center).
    /// </summary>
    [Tooltip("GUI X-axis offset for the button (from the center).")]
    public int GUIXOffset;

    /// <summary>
    /// GUI Y-axis offset for the button (from the bottom).
    /// </summary>
    [Tooltip("GUI Y-axis offset for the button (from the bottom).")]
    public int GUIYOffset;

    private bool buttonPressed;

    private int buttonWidth = 200;
    private int buttonHeight = 20;

    private Vector3 startPos;

	// Use this for initialization
	void Start () {
        startPos = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		
        if (buttonPressed)
        {
            buttonPressed = false;
            CmdResetBall();
        }

        if (Input.GetKeyDown(ToggleKey))
        {
            ShowGUI = !ShowGUI;
        }
	}

    /// <summary>
    /// Commands the server to invoke the RPC call for resetting the ball's position.
    /// </summary>
    [Command]
    void CmdResetBall()
    {
        RpcResetBall();
    }

    /// <summary>
    /// Resets the ball position, and also its velocity by enabling and disabling 
    /// its rigidbody's isKinematic.
    /// </summary>
    [ClientRpc]
    void RpcResetBall()
    {
        transform.position = startPos;

        // Remove velocity
        GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<Rigidbody>().isKinematic = false;
    }

    // Unity-invoked GUI method
    void OnGUI()
    {
        if (!isServer) return;
        if (!ShowGUI) return;

        int buttonXPos = (Screen.width/2)-(buttonWidth/2) + GUIXOffset;
        int buttonYPos = Screen.height + GUIYOffset;

        buttonPressed = GUI.Button(new Rect(buttonXPos, buttonYPos, buttonWidth, buttonHeight), "Reset Ball");
    }
}
