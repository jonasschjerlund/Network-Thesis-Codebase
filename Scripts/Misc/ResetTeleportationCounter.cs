using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Finds a teleportation counter singleton and resets it
/// Useful for scenes that use event calls but only get a teleportation counter at runtime.
/// </summary>
public class ResetTeleportationCounter : SingletonMonoBehaviour<ResetTeleportationCounter> {

	public void Invoke()
    {
        TeleportationCounter.Instance.Reset();
    }
}
