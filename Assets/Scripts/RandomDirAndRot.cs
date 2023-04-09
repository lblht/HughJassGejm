using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// made by chatgpt
public class RandomDirAndRot : MonoBehaviour
{
    public float minForce;
    public float maxForce;
    public float maxTorque;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        AddRandomForce();
    }

    void AddRandomForce()
    {
        Vector3 forceDirection = Random.onUnitSphere; // Random direction
        float forceMagnitude = Random.Range(minForce, maxForce); // Random force magnitude
        Vector3 force = forceDirection * forceMagnitude;
        rb.AddForce(force, ForceMode.Impulse);

        Vector3 torque = Random.insideUnitSphere * maxTorque; // Random torque
        rb.AddTorque(torque, ForceMode.Impulse);
    }
}
