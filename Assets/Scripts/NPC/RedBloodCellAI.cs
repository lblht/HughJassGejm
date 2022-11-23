using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RedBloodCellAI : MonoBehaviour
{
    [SerializeField] private NavMeshAgent navMeshAgent;     // referencia na NavMesh Agent

    void Start()
    {
        navMeshAgent.destination = new Vector3(-20f, 0f, 20f);
        Destroy(gameObject, 20f);
    }
}
