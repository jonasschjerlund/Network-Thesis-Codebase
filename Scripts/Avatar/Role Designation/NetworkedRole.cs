using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Valve.VR.InteractionSystem;

/// <summary>
/// Network implementation for a user's role.
/// </summary>
public class NetworkedRole : NetworkBehaviour {

    // TODO: Clean ConnectionID

    /// <summary>
    /// Contains numbers ranging from one to four.
    /// </summary>
    public enum Player { One, Two, Three, Four }

    // [SyncVar]
    // private int ConnectionID = -1;

    [SerializeField]
    private Player role;

    public Player Role {
        get
        {
            return role;
        }
    }

    public override void OnStartLocalPlayer()
    {
        if (isLocalPlayer)
        {
            StartCoroutine(SetConnectionID(connectionToServer));
        }
    }

    [Command]
    void CmdSetConnectionID(int connectionId)
    {
        RpcSetConnectionID(connectionId);
    }

    [ClientRpc]
    void RpcSetConnectionID(int connectionId)
    {
        // ConnectionID = connectionId;

        // Enums are represented as 0-indexed ints by default, matching UNET connection Ids
        role = (Player)connectionId;

        // Append avatar game object name with user's role ID
        transform.root.gameObject.name += " (User " + Role + ")";
    }

    /// <summary>
    /// Waits until the connection ID is ready, then commands server to assign a role based on the connection ID.
    /// </summary>
    /// <param name="networkConnection">Network connection to assign ID from.</param>
    /// <seealso cref="CmdSetConnectionID(int)"/>
    public IEnumerator SetConnectionID(NetworkConnection networkConnection)
    {
        yield return new WaitUntil(() => networkConnection.isReady);

        CmdSetConnectionID(networkConnection.connectionId);
        DefineRole(networkConnection.connectionId);
    }

    /// <summary>
    /// Defines key functionality for a player based on their connection id (0-indexed starting with
    /// first player). Note that most of the changes are local only.
    /// </summary>
    /// <param name="connectionId">Connection id corresponding to a player.</param>
    private void DefineRole(int connectionId)
    {
        switch ((Player) connectionId)
        {
            // The "small" user
            case Player.One:
                gameObject.AddComponent<PlayerOneRole>();
                break;

            // The "large" user
            case Player.Two:
                gameObject.AddComponent<PlayerTwoRole>();
                break;

            default:
                Debug.LogError("Tried to access a role that hasn't been implemented.");
                break;
        }
    }
}