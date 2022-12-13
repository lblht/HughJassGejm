using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
   
    public PowerUpEffect powerupEffect;
    

    private void OnTriggerEnter(Collider collision)
    {

        if(collision.gameObject.tag=="Player")
        {
            Destroy(gameObject);
            powerupEffect.Apply(collision.gameObject);
            
            
        }

        
    }


    


}
