using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BacteriaAI : MonoBehaviour
{
    [SerializeField] private NavMeshAgent navMeshAgent;
    [SerializeField] private float slowSpeed;
    [SerializeField] private float mediumSpeed;
    [SerializeField] private float runSpeed;
    [SerializeField] private float playerDetectionRadius;
    [SerializeField] private float foodRadius;
    [SerializeField] private LayerMask foodMask;        

    private Transform player;
    private Vector3 targetPosition;
    private enum States {LookingForFood, MovingToFood, Eating, Running};
    private States state;
    private float timer;

    void Start()
    {
        state = States.LookingForFood;
        targetPosition = RandomTargetPosition();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void FixedUpdate()
    {
        if(CheckIfPlayerInRange() && state != States.Running)
            state = States.Running;
        
        switch (state)
        {
        case States.LookingForFood:
            LookingForFood();
            break;
        case States.MovingToFood:
            MovingToFood();
            break;
        case States.Eating:
            Eating();
            break;
        case States.Running:
            Running();
            break;
        default:
            LookingForFood();
            break;
        }

        navMeshAgent.destination = targetPosition;
    }

    void LookingForFood()
    {
        navMeshAgent.speed = slowSpeed;

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, foodRadius, foodMask);
        if(hitColliders.Length > 0)
        {
            targetPosition = FindClosestFood(hitColliders);
            state = States.MovingToFood;
            return;
        }

        if(Vector3.Distance(targetPosition, transform.position) < 1f || Time.time > timer)
        {
            targetPosition = RandomTargetPosition();
            timer = Time.time + 10;
        }
    }

    void MovingToFood()
    {
        navMeshAgent.speed = mediumSpeed;
        
        if(Vector3.Distance(targetPosition, transform.position) < 1)
        {
            state = States.Eating;
        }
    }

    void Eating()
    {
        navMeshAgent.speed = 0;

        Debug.Log("Mňam");
    }

    void Running()
    {
        navMeshAgent.speed = runSpeed;

        if(CheckIfPlayerInRange())
        {
            NavMeshHit hit;
            NavMesh.SamplePosition(transform.position + ((transform.position - player.position).normalized * 5), out hit, 3f, NavMesh.AllAreas);
            targetPosition = hit.position;
        }
        else
        {
            state = States.LookingForFood;
        }
    }

    Vector3 RandomTargetPosition()
    {
        float radius = 20;
        Vector3 randomPosition = Random.insideUnitSphere * radius;
        randomPosition += transform.position;
        NavMeshHit hit;
        NavMesh.SamplePosition(randomPosition, out hit, radius, 1);
        return hit.position;
    }

    Vector3 FindClosestFood(Collider[] hitColliders)
    {
        float closestDistacne = Vector3.Distance(hitColliders[0].transform.position, transform.position);
        Vector3 targetPosition = hitColliders[0].transform.position;
        foreach (var hitCollider in hitColliders)
        {
            float distance = Vector3.Distance(hitCollider.transform.position, transform.position);
            if(distance < closestDistacne)
            {
                targetPosition = hitCollider.transform.position;
                closestDistacne = distance;
            }
        }

        return targetPosition;
    }

    bool CheckIfPlayerInRange()
    {
        if(Vector3.Distance(player.position, transform.position) < playerDetectionRadius)
            return true;
        else
            return false;
    }


    //DEBUG
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, foodRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, playerDetectionRadius);
    }
}
