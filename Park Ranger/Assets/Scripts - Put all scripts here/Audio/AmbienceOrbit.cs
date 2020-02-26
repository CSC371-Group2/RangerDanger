using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbienceOrbit : MonoBehaviour
{
    public float orbitDistance;
    public float degreePerSec;
    private Transform target;

    private void Awake()
    {
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    private void LateUpdate()
    {
        orbit();
    }

    void orbit()
    {
        if (target != null)
        {
            transform.position = target.position + (transform.position - target.position).normalized * orbitDistance;
            transform.RotateAround(target.position, Vector3.up, degreePerSec * Time.deltaTime);
        }
    }
}
