using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Rendering.PostProcessing;
using Valve.VR.InteractionSystem;

/// <summary>
/// Defines behavior for the second player to connect to the server.
/// </summary>
public class PlayerTwoRole : MonoBehaviour {

	// Use this for initialization
	void Start () {

        switch (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name)
        {
            case "Bedroom Scene":
                Player.instance.transform.position = IndoorsRoomKeyObjectsReference.Instance.SmallPlayerStartingPosition.position;
                GetComponent<GenericScaler>().CmdApplyScale(IndoorsRoomKeyObjectsReference.Instance.SmallPlayerStartingSize);
                Teleport.instance.arcDistance = IndoorsRoomKeyObjectsReference.Instance.TeleportArcDistance;
                IndoorsRoomKeyObjectsReference.Instance.DrawerParentObject.GetComponent<LinearDrive>().enabled = false;
                Player.instance.transform.parent = GameObject.Find("Main Drawer").transform;
                break;

            case "Bedroom Scene Mirrored":
                Player.instance.transform.position = IndoorsRoomKeyObjectsReference.Instance.LargePlayerStartingPosition.position;
                GetComponent<GenericScaler>().CmdApplyScale(IndoorsRoomKeyObjectsReference.Instance.LargePlayerStartingSize);
                IndoorsRoomKeyObjectsReference.Instance.LargePlayerTeleportAreas.SetActive(true);
                GetComponent<Authority>().CmdRequestAuthority(IndoorsRoomKeyObjectsReference.Instance.DrawerParentObject.GetComponent<NetworkIdentity>());
                break;

            case "Gym Scene":
                Player.instance.transform.position = Vector3.zero;
                break;

            default:
                Debug.LogWarning("No role defined for player two in this scene.");
                break;
        }

        
    }

    void SetThrowablesActive(IEnumerable<Throwable> throwables, bool state)
    {
        foreach (Throwable throwable in throwables)
        {
            throwable.active = state;
        }
    }

    void AssertAuthority(IEnumerable<AuthorityHandler> authorityHandlers)
    {
        foreach (AuthorityHandler authorityHandler in authorityHandlers)
        {
            authorityHandler.CmdAssignAuthority(GetComponent<NetworkIdentity>());
        }
    }

}
