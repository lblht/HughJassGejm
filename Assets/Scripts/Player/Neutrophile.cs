using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Neutrophile : MonoBehaviour
{
    public Transform poisonSpawnPoint;
    public GameObject poisonArea;

    public GameObject poisonEmitArea;
    public ParticleSystem poisonEmitEffect;

    public float cooldownTime=10;
    public int maxPoisonAmmo = 3;
    
    private bool poisonCharged = true;


    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {  
           poisonEmitArea.SetActive(true);
           poisonEmitEffect.Play();
        }

        if(Input.GetMouseButtonUp(0))
        {  
           poisonEmitArea.SetActive(false);
           poisonEmitEffect.Stop();
        }

        if((Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.E)) && poisonCharged)
        {  
            ReleasePoison();  
            StartCoroutine(ReloadPoison());
        }
    }

    IEnumerator ReloadPoison()
    {
        poisonCharged = false;

        Debug.Log("Reloading Poison");

        yield return new WaitForSeconds(cooldownTime);

        poisonCharged = true;
    }

    void ReleasePoison()
    {
        var poison = Instantiate(poisonArea, poisonSpawnPoint.position, poisonSpawnPoint.rotation);
    }
}
