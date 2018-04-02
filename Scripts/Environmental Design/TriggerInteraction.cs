using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class TriggerInteraction : MonoBehaviour {

    public NetworkedRole.Player PlayerRoleToCheck;

    public bool Lock;

    public UnityEvent OnEnter;

	void OnTriggerEnter(Collider other)
    {
        if (Lock) return;

        if (other.transform.root.gameObject.CompareTag("Player") || other.transform.root.gameObject.CompareTag("LocalAvatar"))
        {
            if (other.transform.root.gameObject.GetComponent<NetworkedRole>().Role == PlayerRoleToCheck)
            {
                if (OnEnter != null)
                {
                    OnEnter.Invoke();
                }
                Lock = true;
            }
        }
    }
}