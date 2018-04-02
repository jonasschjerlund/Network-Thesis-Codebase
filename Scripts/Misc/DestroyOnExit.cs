using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Valve.VR.InteractionSystem;

/// <summary>
/// Destroys an object when it exits a particular area as defined by a collider.
/// </summary>
public class DestroyOnExit : MonoBehaviour {

    public Collider ColliderToCheck;

    public UnityEvent OnDestroy;

    private Hand hand;

	// Use this for initialization
	void Start () {
        if (!ColliderToCheck.isTrigger)
        {
            Debug.LogWarning("Collider is not a trigger. DissolveOnExit won't function.");
        }

	}
	
    void OnAttachedToHand(Hand hand)
    {
        this.hand = hand;
    }

    void OnDetachedFromHand()
    {
        hand = null;
    }

	void OnTriggerEnter(Collider other)
    {
        if (other == ColliderToCheck)
        {
            if (hand != null)
            {
                hand.DetachObject(gameObject);
            }

            Debug.Log("Shiieet nigga");
            OnDestroy.Invoke();
            Destroy(gameObject);
        }
    }
}
