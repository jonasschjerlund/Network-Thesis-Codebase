using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class LoadCurrentServerScene : NetworkBehaviour {

    public override void OnStartLocalPlayer()
    {
        if (NetworkManager.networkSceneName != string.Empty && SceneManager.GetActiveScene().name != NetworkManager.networkSceneName)
        {
            SceneManager.LoadScene(NetworkManager.networkSceneName);
        }
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
