using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Quits the application when a specific key is pressed.
/// </summary>
public class QuitApplicationOnKeyPress : MonoBehaviour {

    /// <summary>
    /// Quit application when this key is pressed.
    /// </summary>
    [Tooltip("Quit application when this key is pressed.")]
    public KeyCode Key = KeyCode.Escape;
    
    // Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(Key))
        {
            Application.Quit();
        }
	}
}