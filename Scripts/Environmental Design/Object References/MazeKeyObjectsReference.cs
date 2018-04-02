using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Valve.VR.InteractionSystem;

/// <summary>
/// Singleton container for key objects to be manipulated by the roles.
/// </summary>
public class MazeKeyObjectsReference : SingletonMonoBehaviour<MazeKeyObjectsReference> {

    [Header("Big player")]
    [Tooltip("Scaling factor for the \"big\" player.")]
    public float BigPlayerSize = 8.0f;

    public GameObject TeleportPoints;

    public GameObject HugeRock;

    [Header("Small player")]
    public GameObject TeleportAreas;

    public Transform SmallPlayerSpawnPoint;

    public GameObject Ceiling;

    public List<Throwable> Throwables;

    public List<AuthorityHandler> AuthorityHandlers;

    void Start()
    {
        Throwables = new List<Throwable>();
        AuthorityHandlers = new List<AuthorityHandler>();
    }
}
