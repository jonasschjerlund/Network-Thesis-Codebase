using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class DestroyParticleSystemWithDelay : MonoBehaviour {

    float timer;
    ParticleSystem[] particleSystems;

    // Use this for initialization
    void Start () {
        particleSystems = GetComponentsInChildren<ParticleSystem>();
	}
	
	// Update is called once per frame
	void Update () {
        timer += Time.deltaTime;

        // Before we start assessing the state of all particle systems, check duration for one
        if (timer > particleSystems[0].main.duration)
        {
            foreach (ParticleSystem particleSystem in particleSystems)
            {
                if (!particleSystem.isPlaying)
                {
                    Destroy(gameObject);
                }
            }
        }
	}
}
