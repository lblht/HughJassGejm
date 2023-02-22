using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class Rocket : MonoBehaviour
{
    public float life = 10f;
    public float explosionRadius = 2f;
    public Transform explosionPrefab;
    
    
    void Awake()
    {
        
        //Destroy(gameObject,life);
    }
 
    void OnCollisionEnter(Collision collision)
    {

        Instantiate(explosionPrefab, transform.position, transform.rotation);
        Collider[] colliders = Physics.OverlapSphere(transform.position,explosionRadius);
        foreach(Collider c in colliders)
        {
            
             if(c.gameObject.tag=="Bacteria")
                c.gameObject.GetComponent<BacteriaDeath>().Die("TCell");
        }

        
        Destroy(gameObject);
    }


   

    


}