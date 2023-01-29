using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonArea : MonoBehaviour
{

    public float time = 10.0f;
    public float damageTime = 1f;
    public int damage = 1;

    void Start()
    {
        Invoke("destroyCloud", time);
        Invoke("poisonDamage",damageTime);
    }

    void FixedUpdate()
    {
       
    }
    
    void poisonDamage()
    {        


        Collider[] colliders = Physics.OverlapSphere(transform.position,5f);
        foreach(Collider c in colliders)
        {
            if(c.gameObject.tag=="Bacteria")
            {
                
                c.gameObject.GetComponent<BacteriaDeath>().takeDamage(damage,"Neutrophile");
            }

        }
        Invoke("poisonDamage",damageTime);

        
    }
    

    void destroyCloud()
    {

        Destroy(gameObject);
    }
}
