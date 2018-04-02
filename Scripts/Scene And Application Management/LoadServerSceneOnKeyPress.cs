using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using Valve.VR.InteractionSystem;

public class LoadServerSceneOnKeyPress : NetworkBehaviour
{

    /// <summary>
    /// Change the scene when this key is pressed on the server.
    /// </summary>
    [Tooltip("Change the scene when this key is pressed on the server.")]
    public KeyCode LoadSceneOnPress = KeyCode.Space;

    /// <summary>
    /// Name of the scene to change to.
    /// </summary>
    [Tooltip("Name of the scene to change to.")]
    public string SceneName;

    void Update()
    {
        if (!isServer) return;

        if (Input.GetKeyDown(LoadSceneOnPress))
        {
            CmdResetLocalPlayerObjects();
            NetworkManager.singleton.ServerChangeScene(SceneName);
        }
    }

    [Command]
    void CmdResetLocalPlayerObjects()
    {
        RpcResetLocalPlayerObjects();
    }

    [ClientRpc]
    void RpcResetLocalPlayerObjects()
    {
        Player.instance.transform.parent = null;
        DontDestroyOnLoad(Player.instance.gameObject);
    }
}
