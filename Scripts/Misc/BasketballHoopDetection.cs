using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Checks hoop collision for successful baskets.
/// </summary>
public class BasketballHoopDetection : MonoBehaviour {

    /// <summary>
    /// Colliders that will be checked for hoop collision.
    /// </summary>
    [Tooltip("Colliders that will be checked for hoop collision.")]
    public Collider[] HoopColliders;

    /// <summary>
    /// Explosion effect for successful baskets.
    /// </summary>
    [Tooltip("Explosion effect for successful baskets.")]
    public GameObject SmallExplosionEffectPrefab;

    private Rigidbody rb;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
	}

    void OnTriggerEnter(Collider other)
    {
        if (HoopColliders.Contains(other))
        {
            if (rb.velocity.y < 0)
            {
                Instantiate(SmallExplosionEffectPrefab, other.transform.position, Quaternion.identity);
            }
        }
    }
}
