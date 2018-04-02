using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class LerpEyeRotation : MonoBehaviour
{
    public Transform target;
    public float speed = 10F;

    void Update()
    {
        transform.rotation = Quaternion.Lerp(transform.rotation, target.rotation, Time.deltaTime * speed);
        transform.position = target.position;
    }
}