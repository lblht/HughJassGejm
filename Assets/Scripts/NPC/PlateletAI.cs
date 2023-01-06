using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlateletAI : MonoBehaviour
{
    [SerializeField] private NavMeshAgent navMeshAgent;     // referencia na NavMesh Agent
    [SerializeField] private Vector3 targetPosition;     

    private bool up;

    void Start()
    {
        targetPosition = new Vector3(10f, 0f, -8f);
        navMeshAgent.destination = targetPosition;
    }

    void FixedUpdate()
    {
        if(Vector3.Distance(transform.position, targetPosition) < 2f)
        {
            up = true;
            navMeshAgent.enabled = false;
        }

        if(up)
        {
            transform.position += Vector3.up * 0.2f;
        }
    }
}
