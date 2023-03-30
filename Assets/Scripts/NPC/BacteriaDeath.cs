using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BacteriaDeath : MonoBehaviour
{
    [SerializeField] private int dropChance; // Šanca v % či z bakterie padne časť z jej teľa ( neskôr zbierané dendritic cell)
    [SerializeField] private GameObject part; // prefab objektu časti baktérie (dna)

    private int maxHp = 5;
    private int currentHp;

    void Start()
    {
        currentHp = maxHp;
    }

    // funkcia na zabitie baktérie (používať túto nie iba Destroy()),
    // vyžaduje argument názvu charakteru, ktorý ju zabil: "Macrophage", "Neutrophile", "TCell"... pre updatnutie správneho objectivu
    public void Die(string responsible)
    {
        Drop();
        GameManager.instance?.RemoveBacteria(responsible); // baktériu z hry treba vymazať týmto spôsobom - updatne ui, updatne counter atd...
        Destroy(gameObject);
    }

    // funkcia urěná na poškodenie baktérie pre postavy ktoré ju nezabijú instantne
    // taktiež vyždajuje okrem argumentu veľkosti poškodenia (demage), aj argument charakteru, ktorý ju zabil
    public void takeDamage(int damage, string responsible)
    {
        currentHp = currentHp-damage;
        if(currentHp <= 0)
        {
            Die(responsible);
        }
        Debug.Log(currentHp);
    }


    // funkcia sa volá vždy iba pri smrti baktérie, vyberie náhodné číslo od 1 do 100
    // ak je číslo menšie alebo rovné ako dropChance (šanca na dropnutie časti tela baktérie (dna))
    // spawne inštanciu objektu dna baktérie
    private void Drop()
    {
        if(Random.Range (1, 101) <= dropChance)
        {
            Instantiate(part, transform.position, Quaternion.identity);
        }
    }
}
