using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// 10, 0, -8
public class PlateletAI : MonoBehaviour
{
    [SerializeField] private NavMeshAgent navMeshAgent;     // referencia na NavMesh Agent
    [SerializeField] private Transform targetTransform;     

    private bool up;

    void Start()
    {
        navMeshAgent.destination = targetTransform.position;
    }

    void FixedUpdate()
    {
        if(Vector3.Distance(transform.position, targetTransform.position) < 2f)
        {
            up = true;
            navMeshAgent.enabled = false;
        }

        if(up)
        {
            transform.position += Vector3.up * 0.2f;
        }
    }

    public void SetTargetTransform(Transform target)
    {
        targetTransform = target;
    }
}
