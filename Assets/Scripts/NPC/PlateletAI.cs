using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// 10, 0, -8
public class PlateletAI : MonoBehaviour
{
    [SerializeField] private NavMeshAgent navMeshAgent;     // referencia na NavMesh Agent
    [SerializeField] private Transform targetTransform;     // cieľová pozícia ku ktorej počas celej existencie smeruje

    private bool up; // premenná ktorá hovorí či sa doštička dostala pod ranu a má sa pohybovť smerom hore do rany 

    void Start()
    {
        // nastavenie cieľovej destinácie do navmesh agenta
        navMeshAgent.destination = targetTransform.position;
    }

    void FixedUpdate()
    {
        // kontroluje či je vzdialenosť k cieľu menšia ako 2 (dostala sa k rane), ak ano tak sa premnná up zmení na true a nevmesh agent sa vypne
        if(Vector3.Distance(transform.position, targetTransform.position) < 2f)
        {
            up = true;
            navMeshAgent.enabled = false;
        }

        // ak je premenná up true tak sa doštička pohybuje rovno hore (natvrdo sa mení jej pozícia)
        if(up)
        {
            transform.position += Vector3.up * 0.2f;
        }
    }

    // po spawne zo spawn skriptu nastavujeme cieľovú destnáciu aby to nebolo zadané natvrdo aka. každý spawner môže posielať doštičky na iné miesto
    public void SetTargetTransform(Transform target)
    {
        targetTransform = target;
    }
}
