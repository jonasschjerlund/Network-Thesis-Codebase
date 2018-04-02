using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UsernameGUI : MonoBehaviour {

    public bool ShowGUI;

    public KeyCode ToggleKey = KeyCode.T;

    public int XOffset;
    public int YOffset;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
        if (Input.GetKeyDown(ToggleKey))
        {
            ShowGUI = !ShowGUI;
        }
        	
	}

    void OnGUI()
    {
        if (!ShowGUI) return;



        int xpos = 10 + XOffset;
        int ypos = 40 + YOffset;

        DataSystemController.Instance.Username = GUI.TextField(new Rect(xpos + 100, ypos, 200, 20), DataSystemController.Instance.Username);
    }


}
