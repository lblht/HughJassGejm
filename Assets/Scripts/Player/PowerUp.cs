using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [SerializeField] bool speed;
    [SerializeField] bool dash;
    [SerializeField] MeshRenderer mesh;
    [SerializeField] BoxCollider collider;
    [SerializeField] ParticleSystem effect;

    private void OnTriggerEnter(Collider collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            if(speed)
                collision.gameObject.GetComponent<PowerUpApplier>().SpeedBoost();
            if(dash)
                collision.gameObject.GetComponent<PowerUpApplier>().Dash();
            Deactivate();
            Invoke("Activate", 20f);
        }  
    }

    void Activate()
    {
        mesh.enabled = true;
        collider.enabled = true;
    }

    void Deactivate()
    {
        effect.Play();
        mesh.enabled = false;
        collider.enabled = false;
    }
}
