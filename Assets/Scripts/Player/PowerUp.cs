using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// na začiatok by som chcel iba povedať že som si veľmi vedomí ako zle je tento skript navrhnutý, ale funugje... 
public class PowerUp : MonoBehaviour
{
    [SerializeField] bool speed; // premenná ktorá udáva či je tento powerup speed power
    [SerializeField] bool dash; // premenná udáva či je tento powerup dash power up
    [SerializeField] MeshRenderer mesh; // mesh powerup 
    [SerializeField] BoxCollider collider;  
    [SerializeField] ParticleSystem effect; // efekt ktorý sa prehrá po zdvihnutí power upu

    // funkcia sa zavolá ak objekt prejde cez power up
    private void OnTriggerEnter(Collider collision)
    {
        // ak je objekt ktorý cez neho prešiel je hráč
        if(collision.gameObject.tag == "Player")
        {
            // zavolá sa funkcia na scripte PowerUpApplier ktorý sa nachádza na objekte hráča podľa toho aký druh power upu toto je
            if(speed)
                collision.gameObject.GetComponent<PowerUpApplier>()?.SpeedBoost();
            if(dash)
                collision.gameObject.GetComponent<PowerUpApplier>()?.Dash();
            // následne sa objekt power upu deaktivuje
            Deactivate();
            // a znovu sa aktivuje za daný čas
            Invoke("Activate", 20f);
        }  
    }

    // fukvia ktorá aktivuje mesh a collider power upu
    void Activate()
    {
        mesh.enabled = true;
        collider.enabled = true;
    }

    // funkcia deatkivuje mesh a collider power upu (aby nemohol byť znovu hned zdvihnutý) a prehrá efekt zdvihnutia
    void Deactivate()
    {
        effect.Play();
        mesh.enabled = false;
        collider.enabled = false;
    }
}
