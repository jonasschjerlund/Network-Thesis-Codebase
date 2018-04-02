using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Readjusts the bounciness of a game object on entry and exit.
/// </summary>
public class BallBouncyAdjustment : MonoBehaviour {

    /// <summary>
    /// Names of game object that this trigger should affect.
    /// </summary>
    [Tooltip("Names of game object that this trigger should affect.")]
    public string NameToCheck;

    /// <summary>
    /// Value to adjust the bounciness of entering objects to.
    /// </summary>
    [Tooltip("Value to adjust the bounciness of entering objects to.")]
    public float AdjustedBounciness = 0.1f;

    private float originalBounciness;

    void OnTriggerEnter(Collider other)
    {
        if (other.name.StartsWith(NameToCheck))
        {
            originalBounciness = other.material.bounciness;
            other.material.bounciness = AdjustedBounciness;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.name.StartsWith(NameToCheck))
        {
            other.material.bounciness = originalBounciness;
        }
    }
}
