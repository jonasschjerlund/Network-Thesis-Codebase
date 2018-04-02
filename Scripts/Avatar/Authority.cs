using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// Component that can assign and remove authority for a given client.
/// </summary>
public class Authority : NetworkBehaviour {

    [SerializeField]
    public NetworkIdentity localPlayerNetworkIdentity;

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();
        localPlayerNetworkIdentity = GetComponent<NetworkIdentity>();
    }

    /// <summary>
    /// Commands the server to invoke a RPC call for client authority.
    /// </summary>
    /// <param name="objectNetworkIdentity">Object to request authority over.</param>
    [Command]
    public void CmdRequestAuthority(NetworkIdentity objectNetworkIdentity)
    {
        RpcAssignClientAuthority(objectNetworkIdentity);
    }

    /// <summary>
    /// Invoked via a command. Removes existing client authority owner if one exists,
    /// then assigns authority to the local client connection.
    /// </summary>
    /// <param name="objectNetworkIdentity">Object to assign authority over.</param>
    [ClientRpc]
    void RpcAssignClientAuthority(NetworkIdentity objectNetworkIdentity)
    {
        if (objectNetworkIdentity.clientAuthorityOwner != null)
        {
            objectNetworkIdentity.RemoveClientAuthority(objectNetworkIdentity.clientAuthorityOwner);
        }

        objectNetworkIdentity.AssignClientAuthority(connectionToClient);
    }

    /// <summary>
    /// Commands the server to send out an RPC that removes authority from a specific object.
    /// </summary>
    /// <param name="objectNetworkIdentity">Object to remove authority from.</param>
    [Command]
    public void CmdRequestAuthorityRemoval(NetworkIdentity objectNetworkIdentity)
    {
        RpcRemoveAuthority(objectNetworkIdentity);
    }

    /// <summary>
    /// Removes authority from a specific object, if it has an owner.
    /// </summary>
    /// <param name="objectNetworkIdentity">Object to remove authority from.</param>
    [ClientRpc]
    void RpcRemoveAuthority(NetworkIdentity objectNetworkIdentity)
    {
        if (objectNetworkIdentity.clientAuthorityOwner != null)
        {
            objectNetworkIdentity.RemoveClientAuthority(objectNetworkIdentity.clientAuthorityOwner);
        }
    }

}
