using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//komentar jak blazen
public class BacteriaAI : MonoBehaviour
{
    
    [SerializeField] private Animator animator;   
    [SerializeField] private NavMeshAgent navMeshAgent;     // referencia na NavMesh Agent
    [SerializeField] private float slowSpeed;               // rýchlosť ked hľadá jednlo
    [SerializeField] private float mediumSpeed;             // rýchlosť ked sa pohybuje k jednlu
    [SerializeField] private float runSpeed;                // rýchlosť ked uteká pred hráčom
    [SerializeField] private float playerDetectionRadius;   // radius detekcie hráča
    [SerializeField] private LayerMask playerMask;          // colízna vrstva hráča
    [SerializeField] private float foodRadius;              // rádius detekcie jedla
    [SerializeField] private LayerMask foodMask;            // colízna vrstva jednla
    [SerializeField] private float eatingDelay;             // delay medzi každým odkusnutím z jedla
    [SerializeField] private int eatingAmount;              // veľkosť odkusnutia
    [SerializeField] private int amountToReproduce;         // množstvo jedla ktoré musí skonzumovať na mitozu
    [SerializeField] private Transform mesh;                // referencia na transform meshu
    [SerializeField] private GameObject bacteriaPrefab;     // prefab baktérií ktoré sa spawnú po mitoze
    [SerializeField] private LayerMask bacteriaLayer;   

    private Transform player;       // referencia na pozíciu hráča
    private Vector3 targetPosition; // pozícia ku ktorej sa práve pohybuje
    private enum States {LookingForFood, MovingToFood, Eating, Running};    // deklaracia stavov
    private States state;           
    private float moveTimer;        // časovač po ktorom si najde novú targer pozíciu, keby sa náhodou k tej predošlej nevie dostať 
    private Food currentFood;       // referencia na momentálne jedlo
    private float eatTimer;         // uchováva hodnotu času ďalšieho odkusnutia
    private int resourceAmount;     // množstvo jedla ktoré zjedol
    private float ragdollTimer;

    void Start()
    {
        GameManager.instance?.AddBacteria();
        state = States.LookingForFood;              // nastavenie stavu v ktorom začína
        targetPosition = RandomTargetPosition();
        navMeshAgent.enabled = false;
    }

    void FixedUpdate()
    {
        if(CheckIfPlayerInRange() && state != States.Running)   // stále kontroluje či je vzdialenosť od hráča dostatočne malá
        {
            state = States.Running;                             // ak ano prepne sa do stavu utekania
            if(currentFood != null)
            {
                currentFood.SetOccupied(false);
                currentFood = null;
            }
        }
        // ktorý podla momentálneho stavu volá príslušné funkcie
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
            break;
        }

        if(navMeshAgent.enabled == false && Time.time > ragdollTimer)     // dočasná poistka
        {
            Ragdoll(false, 0f);
            targetPosition = RandomTargetPosition();
        }

        if(navMeshAgent.enabled == true)
            navMeshAgent.destination = targetPosition;      // nastavenie destinácie to NavMesh Agenta



        AlignMeshWithGround();
    }
    // funkcia volaná ak je v stave hladania jedla
    void LookingForFood()
    {
        navMeshAgent.speed = slowSpeed;

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, foodRadius, foodMask);  // získa pole colliderov v danom rádiuse ktoré su na vrstve food radius
        if(hitColliders.Length > 0)
        {
            targetPosition = FindClosestFood(hitColliders);     // nájde najbližšie jedno
            state = States.MovingToFood;                        // zmení stav na pohyb k jedlu
            return;
        }

        if(Vector3.Distance(targetPosition, transform.position) < 1f || Time.time > moveTimer)  // ak je vzdialenosť k cielovej pozicii menšia ako 1 alebo vypršal moveTimer
        {
            targetPosition = RandomTargetPosition();
            moveTimer = Time.time + 10;
        }
    }
    // funkcia volaná ak je v stavle presúvania sa k jedlu
    void MovingToFood()
    {
        navMeshAgent.speed = mediumSpeed;

        if(currentFood.GetOccupied())   // stále kontroluje či sa k jedlu nedostala skôr iná baktéria
        {
            state = States.LookingForFood;
            currentFood = null;
        }   
        else if(Vector3.Distance(targetPosition, transform.position) < 1.5f)    // ak je vzdialenosť k jedlu menšia ako 2 zmení sa stav na jedenie
        {
            state = States.Eating;
            eatTimer = Time.time + eatingDelay;
            currentFood.SetOccupied(true);
        }
    }
    // funkcia volaná ak je v stave jedenia
    void Eating()
    {
        navMeshAgent.speed = 0;

        // Aby bola baktéria vždy otočená k jedlu ktoré práva papá
        Vector3 rotDir = Vector3.RotateTowards(transform.forward, currentFood.transform.position - transform.position, Time.deltaTime * 1f, 0.0f);
        transform.rotation = Quaternion.LookRotation(rotDir);

        if(Time.time > eatTimer)
            TakeABite();
    }
    // Funkcia odoberie zdroje z objektu jedla a pridá zdroje (jedlo) baktérii
    void TakeABite()
    {
        if(currentFood.ReduceAmount(eatingAmount))  // Ak ma objekt jedla dostatok zdrojov
        {
            resourceAmount += eatingAmount;
            if(resourceAmount >= amountToReproduce)
            {
                animator.Play("Bacteria_Mitosis");
                return;
            }
            eatTimer = Time.time + eatingDelay;
        }
        else
        {
            state = States.LookingForFood;
            currentFood = null;
        }
    }
    // funkcia sa stará o rozmnožovanie baktérie
    void Mitosis()
    {
        Vector3 position1 = transform.position + (transform.right * 0.35f);    // vypočet pozície pre spawn nových baktérií
        Vector3 position2 = transform.position + (transform.right * -0.35f);
        Reset();
        Instantiate(bacteriaPrefab, position1, transform.rotation);        // inštancovanie nových baktérii
        Instantiate(bacteriaPrefab, position2, transform.rotation);
        Destroy(gameObject);

    }
    // funkcia volaná ak je v stave utekania pred hráčom
    void Running()
    {
        navMeshAgent.speed = runSpeed;

        if(CheckIfPlayerInRange())
        {
            NavMeshHit hit;
            // nájde pozíciu na NavMeshi vo v zdialenosti 5 v okruhu 3 smerom od hráča
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
        Vector3 randomPosition = Random.insideUnitSphere * radius;  // random pozícia v rádiuse 
        randomPosition += transform.position;                       // pripočítanie random pozície k pozícii hráča
        NavMeshHit hit;
        NavMesh.SamplePosition(randomPosition, out hit, radius, 1); // najdenie pozície na NavMeshi v okruhu 1 od vygenerovanej random pozície
        return hit.position;
    }

    Vector3 FindClosestFood(Collider[] hitColliders)
    {
        float closestDistacne = Vector3.Distance(hitColliders[0].transform.position, transform.position);   // nastavenie základnej hodnoty vzdialenosťou k prvemu collideru v poli
        Vector3 targetPosition = hitColliders[0].transform.position;
        currentFood = hitColliders[0].gameObject.GetComponent<Food>();
        // cyklus prejde všetkými collidermi jedla v poli a najde najbližší
        foreach (var hitCollider in hitColliders)
        {
            float distance = Vector3.Distance(hitCollider.transform.position, transform.position);
            if(distance < closestDistacne)
            {
                targetPosition = hitCollider.transform.position;
                closestDistacne = distance;
                currentFood = hitCollider.gameObject.GetComponent<Food>();
            }
        }

        return targetPosition;
    }

    bool CheckIfPlayerInRange()
    {
        /*if(Vector3.Distance(player.position, transform.position) < playerDetectionRadius)
            return true;
        else
            return false;*/

        if(Physics.OverlapSphere(transform.position, playerDetectionRadius, playerMask).Length > 0)
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, playerDetectionRadius, playerMask);
            player = hitColliders[0].transform;
            return true;
        }
        else
            return false;
    }
    // Resetuje hodnoty na default
    void Reset()
    {
        GameManager.instance?.RemoveBacteria("Mitosis");
        currentFood?.SetOccupied(false);
        mesh.localScale = new Vector3(1f,1f,1f);
        animator.Play("Bacteria_Idle");
        //currentFood = null;
        //resourceAmount = 0;
        //state = States.LookingForFood;     
        //targetPosition = RandomTargetPosition();
        //gameObject.SetActive(false);
        // TODO: Pooling?
    }

    void AlignMeshWithGround()
    {
        //TODO
    }

    public void Ragdoll(bool value, float delay)
    {
        GetComponent<Rigidbody>().useGravity = value;
        GetComponent<Rigidbody>().isKinematic = !value;
        GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = !value;
        ragdollTimer = Time.time + delay;
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
