using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class Rocket : MonoBehaviour
{
    public float life = 3;
    public ParticleSystem explosion;
    
    void Awake()
    {
        Destroy(gameObject, life);
    }
 
    void OnCollisionEnter(Collision collision)
    {

        Instantiate(explosion, transform.position, transform.rotation);

        
        Destroy(gameObject);
    }
}