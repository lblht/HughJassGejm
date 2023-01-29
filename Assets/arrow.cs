using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class arrow : MonoBehaviour
{
    public float life = 5;
    public int damage = 5;
    
    void Awake()
    {
        Destroy(gameObject, life);
    }
 
    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Sip trafil");
        

        if(collision.gameObject.tag=="Bacteria")
        {
            Debug.Log("Sip trafil bakteriu");
            collision.gameObject.GetComponent<BacteriaDeath>().takeDamage(damage,"TCell");
        }
        Destroy(gameObject);
    }
}