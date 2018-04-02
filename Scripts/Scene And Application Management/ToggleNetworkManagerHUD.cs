using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ToggleNetworkManagerHUD : MonoBehaviour {

    public KeyCode ToggleKey = KeyCode.T;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(ToggleKey))
        {
            GetComponent<NetworkManagerHUD>().showGUI = !GetComponent<NetworkManagerHUD>().showGUI;
        }
	}
}
