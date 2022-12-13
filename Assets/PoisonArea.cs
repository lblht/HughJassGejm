using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonArea : MonoBehaviour
{

    public float time = 10.0f;

    void Start()
    {
        Invoke("destroyCloud", time);
    }

    void OnCollisionEnter(Collision collision)
    {        

        if(collision.gameObject.tag=="Bacteria")
        {
            Destroy(collision.gameObject);
        }
        
    }

    void destroyCloud()
    {

        Destroy(gameObject);
    }
}
