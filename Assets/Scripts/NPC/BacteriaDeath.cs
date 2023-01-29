using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BacteriaDeath : MonoBehaviour
{
    private int maxHp = 5;
    [SerializeField] private int dropChance;
    [SerializeField] private List<GameObject> parts;

    private int currentHp;

    void Start()
    {
        currentHp = maxHp;
    }


    public void Die(string responsible)
    {
        Drop();
        GameManager.instance?.RemoveBacteria(responsible);
        Destroy(gameObject);
    }

    public void takeDamage(int damage, string responsible)
    {
        currentHp = currentHp-damage;
        if (currentHp<=0)
        {
            Die(responsible);
        }
        Debug.Log(currentHp);
    }

    private void Drop()
    {
        if(Random.Range (1, 101) <= dropChance)
        {
               Instantiate(parts[Random.Range (0, parts.Count)], transform.position, Quaternion.identity);
        }
    }
}
