using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class HighFive : MonoBehaviour {

    public GameObject OtherHand;

    public GameObject HighfiveEffectPrefab;

    public VelocityEstimator VelocityEstimator;


    [Tooltip("Threshold in units/second that must be surpassed before high five event is triggered.")]
    public float CollisionVelocityThreshold = 2.5f;

    public delegate void HighFiveDelegate();
    public event HighFiveDelegate OnHighFive;

    private Vector3 previousPosition;
    

    void Start()
    {
        if (GetComponent<VelocityEstimator>()) VelocityEstimator = GetComponent<VelocityEstimator>();
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.StartsWith("Controller"))
        {
            if (other.gameObject != OtherHand && VelocityEstimator.GetVelocityEstimate().magnitude > CollisionVelocityThreshold)
            {
                // particleManager.CmdSpawnParticleSystemByName(HighfiveEffectPrefab.name, other.ClosestPointOnBounds(transform.position));

                // hand.controller.TriggerHapticPulse(2000);
                
                // TODO: Vibrate this and other game object's controller's haptic!!!

                Instantiate(HighfiveEffectPrefab, other.ClosestPointOnBounds(transform.position), Quaternion.identity);

                if (OnHighFive != null)
                {
                    OnHighFive.Invoke();
                }
            }
        }
    }

    /// <summary>
    /// Compares velocity estimations of two velocity estimators. Returns true if EITHER is above threshold.
    /// </summary>
    /// <param name="threshold"></param>
    /// <param name="thisEstimator"></param>
    /// <param name="otherEstimator"></param>
    /// <returns></returns>
    bool CompareVelocityEstimations(float threshold, VelocityEstimator thisEstimator, VelocityEstimator otherEstimator)
    {
        if (thisEstimator.GetVelocityEstimate().magnitude > threshold || otherEstimator.GetVelocityEstimate().magnitude > threshold)
        {
            return true;
        }
        return false;
    }
}
