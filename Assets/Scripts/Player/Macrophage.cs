using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Macrophage : MonoBehaviour
{
    [SerializeField] private ThirdPersonController thirdPersonController; // referencia na skript ktorý sa stará o všeobecné ovládanie postavy z 3. osoby
    [SerializeField] private LayerMask bacteriaLayer;  // vrstva na ktorej sa nachádzajú baktérie
    [SerializeField] private GameObject suckEffect;   // referencia na objekt na ktorom sa nechádza particle efekt vcucávania
    [SerializeField] private AudioPlayer audioPlayer;  // asi chápeme

    private bool canEat = true; // premenná ktorá určuje či má hráč povolené používať akciu jedenie baktérií 
    private bool eating;        // čí ja akcija jedenie momentálne aktívna

    void Update()
    {
        // v skripte ThirdPersonController je premenná ktorá udáva či má hráč povolené ovládanie postavy
        // tu skontrolujeme či má alebo nemá a aplikujeme to aj na tento skript
        // aka ak má hráč zakázané ovládanie postavy tak tento skript nepríma input
        if(thirdPersonController.GetAllowControl())
            GetInput();
    }

    void FixedUpdate()
    {
        if(eating) // ak eating tak eating... logicky
            Eating();

        suckEffect.SetActive(eating);
    }

    // funkcia ktorá sa stará o prímanie inputu plus nastavuje aj nejaké ďalšie veci lebo to tak bolo jednoduchšie
    // nechcelo sa mi špekulovať s novým input systémom
    void GetInput()
    {
        // ak hráč stlačí E alebo LMB tak sa premenná eating nastaví na true ak hra nieje pauznutá tak sa prehráva aj zvukový efekt 
        if((Input.GetKeyDown(KeyCode.E) || Input.GetMouseButtonDown(0)) && canEat)
        {
            eating = true;
            if(GameManager.instance && !GameManager.instance.GetPause())
                audioPlayer.PlaySound("suck");
        }
        // ak hráč pustí E alebo LMB tak sa eating zmení na false a zastaví sa prehrávanie zvukového efektu
        if(Input.GetKeyUp(KeyCode.E) || Input.GetMouseButtonUp(0))
        {
            audioPlayer.StopSound("suck");
            eating = false;
        }

        // ak postava momentálne vykonáva akciu jedenie ale jedenie je zakázané tak sa jedenie zmení an false
        if(eating && !canEat)
            eating = false;
    }

    // funkcia sa stará o vykonávanie akcie jedenia
    void Eating()
    {
        // skontroluje oblasť pred hráčom a získa všetky collideri v danej oblasti na vestve bektérii
        Collider[] hitColliders = Physics.OverlapSphere(transform.position + (transform.forward * 2f) + (transform.up * - 1f), 2f, bacteriaLayer);

        // prejde všetky nájdené collideri v danej oblasti a proste urobí nejaké čary aby sa bektéria začala pohybovať k hráčovi spôsobom ako keby ju vťahoval
        foreach (var hitCollider in hitColliders)
        {
            hitCollider.gameObject.GetComponent<BacteriaAI>().Ragdoll(true, 5);
            hitCollider.gameObject.GetComponent<Rigidbody>().AddForce(Vector3.up * 5);
            hitCollider.gameObject.GetComponent<Rigidbody>().AddForce((transform.position - hitCollider.transform.position).normalized * 10);
            // ak je baktéria dostatočne blízko hráčovi (môže ju zjesť)
            if(Vector3.Distance(transform.position, hitCollider.transform.position) < 1.8f)
            {
                // zavolá funkciu na baktérii na jej zabitie
                hitCollider.gameObject.GetComponent<BacteriaDeath>().Die("Macrophage");
                // spusti corutinu na efekt zjedenia
                StartCoroutine("Expand");
                // a prehrá zvuk zjedenia
                audioPlayer.PlaySound("eat");
            }
        }
    }

    // efekt zjedenia (natiahne model hráča)
    IEnumerator Expand()
    {
        bool expanding = true;
        bool a = true;
        float size = 1;
        float rate = 2f;

        while(a & expanding)
        {
            size += rate * Time.deltaTime;
            transform.localScale = new Vector3(size, transform.localScale.y, size);
            if(size >= 1.5f)
                a = false;
            yield return null; 
        }

        while(!a & expanding)
        {
            size -= rate * Time.deltaTime;
            transform.localScale = new Vector3(size, transform.localScale.y, size);
            if(size <= 1)
                expanding = false;
           yield return null; 
        }

        yield return null; 
    }


    // DEBUG
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + (transform.forward * 2f) + (transform.up * - 1f), 2f);
    }
}
