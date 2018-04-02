using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Valve.VR.InteractionSystem;

/// <summary>
/// Handles interactable events related to authority and ensures users
/// cannot snatch objects from authority owner.
/// </summary>
[RequireComponent(typeof(NetworkIdentity))]
public class AuthorityHandler : NetworkBehaviour
{

    /// <summary>
    /// Will attempt to assign authority to the network identity of the player avatar that touches this object.
    /// </summary>
    [Tooltip("Will attempt to assign authority to the network identity of the player avatar that touches this object.")]
    public bool AssignAuthorityOnTouch = true;

    private NetworkIdentity objectNetworkIdentity;

    private NetworkIdentity currentOwner;

    [SyncVar]
    public bool AuthorityLocked;

    Throwable throwable;

    // Use this for initialization
    void Start()
    {
        objectNetworkIdentity = GetComponent<NetworkIdentity>();
        throwable = GetComponent<Throwable>();
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.name == "floor")
        {
            // SetAuthorityLocked(false);
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (!AssignAuthorityOnTouch) return;

        if (other.gameObject.name == "Object Pickup Collider" && !AuthorityLocked)
        {
            // CmdAssignAuthority(other.transform.root.GetComponent<NetworkIdentity>());
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (!AssignAuthorityOnTouch) return;

        if (other.gameObject.name == "Object Pickup Collider" && hasAuthority)
        {
            // CmdRemoveAuthority();
        }
    }

    void OnHandHoverBegin()
    {
        if (!AssignAuthorityOnTouch) return;

        // if (!hasAuthority && AuthorityLocked) throwable.active = false;
    }

    void OnHandHoverEnd()
    {
        if (!AssignAuthorityOnTouch) return;

        // if (!throwable.active) throwable.active = true;
    }

    void OnAttachedToHand(Hand hand)
    {
        if (!AssignAuthorityOnTouch) return;

        // CmdSetAuthorityLocked(true);

        GameObject.FindGameObjectWithTag("LocalAvatar").GetComponent<Authority>().CmdRequestAuthority(objectNetworkIdentity);
    }


    void OnDetachedFromHand()
    {
        if (!AssignAuthorityOnTouch) return;

        //CmdSetAuthorityLocked(false);
        // GameObject.FindGameObjectWithTag("LocalAvatar").GetComponent<Authority>().CmdRequestAuthorityRemoval(objectNetworkIdentity);
    }

    [Command]
    void CmdSetAuthorityLocked(bool value)
    {
        AuthorityLocked = value;
    }

    /// <summary>
    /// Assigns authority over this game object to a specific player's 
    /// <see cref="NetworkIdentity"/>.
    /// </summary>
    /// <param name="playerNetworkIdentity">Network identity of player who should gain authority over this game object.</param>
    [Command]
    public void CmdAssignAuthority(NetworkIdentity playerNetworkIdentity)
    {
        RpcAssignAuthority(playerNetworkIdentity);
    }
    [ClientRpc]
    void RpcAssignAuthority(NetworkIdentity playerNetworkIdentity)
    {
        // If player's connection is already the owner, do nothing
        if (objectNetworkIdentity.clientAuthorityOwner == playerNetworkIdentity.connectionToClient)
        {
            return;
        }

        // (Else) if the owner is not null, remove the owner
        if (objectNetworkIdentity.clientAuthorityOwner != null)
        {
            objectNetworkIdentity.RemoveClientAuthority(objectNetworkIdentity.clientAuthorityOwner);
        }

        // Assign authority to the provided id
        objectNetworkIdentity.AssignClientAuthority(playerNetworkIdentity.connectionToClient);
    }

    [Command]
    void CmdRemoveAuthority()
    {
        RpcRemoveAuthority();
    }
    [ClientRpc]
    void RpcRemoveAuthority()
    {
        if (objectNetworkIdentity.clientAuthorityOwner != null)
        {
            objectNetworkIdentity.RemoveClientAuthority(objectNetworkIdentity.clientAuthorityOwner);
        }
    }
}