using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonArea : MonoBehaviour
{
    public float time = 15.0f;
    public float damageTime = 1f;
    public int damage = 1;
    public float radius = 3f;
    public bool autoDestroy;

    void Start()
    {
        if(autoDestroy)
            Invoke("destroyCloud", time);
            
        Invoke("poisonDamage",damageTime);
    }
    
    void poisonDamage()
    {        
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);
        foreach(Collider c in colliders)
        {
            if(c.gameObject.tag=="Bacteria")
            {  
                c.gameObject.GetComponent<BacteriaDeath>().takeDamage(damage,"Neutrophile");
            }

            if(c.gameObject.tag=="Food")
            {  
                c.gameObject.GetComponent<Food>().ReduceAmount(10);
            }
        }
        Invoke("poisonDamage",damageTime);
    }
    
    void destroyCloud()
    {
        Destroy(gameObject);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
