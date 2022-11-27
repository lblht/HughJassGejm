using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BacteriaDeath : MonoBehaviour
{
    [SerializeField] private int dropChance;
    [SerializeField] private List<GameObject> parts;

    public void Die(string responsible)
    {
        Drop();
        GameManager.instance?.RemoveBacteria(responsible);
        Destroy(gameObject);
    }

    private void Drop()
    {
        if(Random.Range (1, 101) <= dropChance)
        {
               Instantiate(parts[Random.Range (0, parts.Count)], transform.position, Quaternion.identity);
        }
    }
}
