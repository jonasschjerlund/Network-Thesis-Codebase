using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableGameObject : MonoBehaviour {

	public void Invoke(GameObject gameObject)
    {
        gameObject.SetActive(false);
    }

    public void Invoke(GameObject[] gameObjects)
    {
        foreach (GameObject gameObject in gameObjects)
        {
            gameObject.SetActive(false);
        }
    }
}
