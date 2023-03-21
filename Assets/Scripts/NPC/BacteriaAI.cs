using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//komentar jak blazen
public class BacteriaAI : MonoBehaviour
{
    
    [SerializeField] private Animator animator;             // referencia na komponent animátor
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
    [SerializeField] private LayerMask bacteriaLayer;       // vrstva na ktorej sa nachádzajú baktérie

    private Transform player;       // referencia na pozíciu hráča
    private Vector3 targetPosition; // pozícia ku ktorej sa práve pohybuje
    private enum States {LookingForFood, MovingToFood, Eating, Running};    // deklaracia stavov
    private States state;           
    private float moveTimer;        // časovač po ktorom si najde novú targer pozíciu, keby sa náhodou k tej predošlej nevie dostať 
    private Food currentFood;       // referencia na momentálne jedlo
    private float eatTimer;         // uchováva hodnotu času ďalšieho odkusnutia
    private int resourceAmount;     // množstvo jedla ktoré zjedol
    private float ragdollTimer;     // čas ako dlho bude baktérie pod vplivom fyziky a nie voládaná komponentom NavMeshAgent

    void Start()
    {
        GameManager.instance?.AddBacteria();        // dá game managerovi vedieť že bola pridaná nová baktéria do hry
        state = States.LookingForFood;              // nastavenie stavu v ktorom začína
        targetPosition = RandomTargetPosition();    // výber náhodnej pozície ku ktorj sa na začiatku začne pohybovať
        navMeshAgent.enabled = false;               // na začiatku sa navmesh agent vypne aby aby bola baktéria od vplivom fyziky (aby padala z rany)
    }

    void FixedUpdate()
    {
         // stále kontroluje či je vzdialenosť od hráča dostatočne malá 
         // ak je a ešte sa nenachádza v stave running tak sa do neho prepne
        if(CheckIfPlayerInRange() && state != States.Running)
        {
            state = States.Running; // zmena stavu na stav running

            // ak je v dostatočnej blízkosti hráča a je zameraný ne nejaké jedlo (predchádzajúci stav bol Eating)
            if(currentFood != null)
            {
                currentFood.SetOccupied(false); // jedlu treba oznámiť že už nieje jedené, aby ďalšia baktéria vedela že sa na ňom môže krmiť
                currentFood = null;             // momentálne vybrané jedno baktérie nastavíme na null (nemá žiadne vybrané, uteká od hráča)
            }
        }

        //switch ktorý podla momentálneho stavu volá príslušné funkcie v ktorých sa nachádza chovanie baktérie v danom stave
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

        // ak je komponent NavMeshAgent vypnutý a čas v ktorom je baktéria ovládaná fyzikov (ragdoll)v vypršal
        // ragdoll sa vypne (zavolá sa funkcie Ragdoll s argumentom false)
        // a vybereie sa nová náhodná pozícia ku ktorej sa začne baktérie pohybovať
        if(navMeshAgent.enabled == false && Time.time > ragdollTimer)  
        {
            Ragdoll(false, 0f);
            targetPosition = RandomTargetPosition();
        }


        // ak je NavMeshAgent zapnutý, jeho destinácia sa nastavuje na targetDestination
        // som si celkom istý že to stačí nastaviť iba raz pri zmene destinácie a nie nastavovať stále v update
        // takže neviem prečo to tu je ale nebudem to radšej meni kým všetko funguje :)
        if(navMeshAgent.enabled == true)
            navMeshAgent.destination = targetPosition;
    }

    // funkcia obsahujúca chovanie baktérie v stave LookingForFood
    void LookingForFood()
    {
        navMeshAgent.speed = slowSpeed; // nastavenie rýchlosti baktérie na pomalú

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, foodRadius, foodMask);  // získa pole colliderov v danom rádiuse ktoré su na vrstve food radius
        if(hitColliders.Length > 0)
        {
            targetPosition = FindClosestFood(hitColliders);     // nájde najbližšie jedlo
            state = States.MovingToFood;                        // zmení stav na pohyb k jedlu
            return;
        }

        if(Vector3.Distance(targetPosition, transform.position) < 1f || Time.time > moveTimer)  // ak je vzdialenosť k cielovej pozicii menšia ako 1 alebo vypršal moveTimer
        {
            targetPosition = RandomTargetPosition();
            moveTimer = Time.time + 10;
        }
    }

    // funkcia obsahujúca chovanie baktérie v stave MovingToFood
    void MovingToFood()
    {
        navMeshAgent.speed = mediumSpeed; // nastavenie rýychlosti baktérie na strednú

        if(currentFood.GetOccupied())   // stále kontroluje či sa k jedlu nedostala skôr iná baktéria
        {
            state = States.LookingForFood; // ak dsotala začne hladať iné jedno
            currentFood = null; // vybrané jedno zmení na null
        }   
        else if(Vector3.Distance(targetPosition, transform.position) < 1.5f)    // ak je vzdialenosť k jedlu menšia ako 2 zmení sa stav na jedenie
        {
            state = States.Eating;
            eatTimer = Time.time + eatingDelay;
            currentFood.SetOccupied(true);
        }
    }

    // funkcia obsahujúca chovanie baktérie v stave Eating
    void Eating()
    {
        navMeshAgent.speed = 0; // nastavenie rýchlosti baktérie na 0, nebohybuje sa pri kŕmení

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
            // myslím že toto asi chápeme nechce sa mi to všerko komentovať :)
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
        Instantiate(bacteriaPrefab, position1, transform.rotation);        // inštancovanie nových baktérii
        Instantiate(bacteriaPrefab, position2, transform.rotation);
        Reset();
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
