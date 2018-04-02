using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Simple helper component that finds the controller model and disables it
/// when it becomes available in the transform hierarchy.
/// </summary>
public class HideControllerModel : MonoBehaviour {
	
	void Start () {
        StartCoroutine(HideModel());
	}
	
    IEnumerator HideModel()
    {
            yield return new WaitUntil(() => transform.Find("BlankController_" + gameObject.name) != null);
            transform.Find("BlankController_" + gameObject.name).gameObject.SetActive(false);
    }
}
