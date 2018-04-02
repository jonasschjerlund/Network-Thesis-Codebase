using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class IndoorsRoomKeyObjectsReference : SingletonMonoBehaviour<IndoorsRoomKeyObjectsReference>
{

    [Header("Drawer")]
    public GameObject DrawerPrefab;
    public Transform StartPosition;
    public Transform EndPosition;

    [Header("Small player")]
    [Tooltip("Scaling factor for the \"small\" player.")]
    public float SmallPlayerStartingSize = 0.05f;

    public Transform SmallPlayerStartingPosition;

    public GameObject SmallPlayerTeleportAreas;

    public GameObject DrawerParentObject;

    public float TeleportArcDistance = 2.16f;

    [Header("Large player")]
    [Tooltip("Scaling factor for the \"large\" player.")]
    public float LargePlayerStartingSize = 1.665f;
    public Transform LargePlayerStartingPosition;
    public GameObject LargePlayerTeleportAreas;

}
