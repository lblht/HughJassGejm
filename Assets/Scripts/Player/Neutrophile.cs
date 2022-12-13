using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Neutrophile : MonoBehaviour
{
    public Transform poisonSpawnPoint;
    public GameObject PoisonArea;

    public float cooldownTime=10;
    public int maxPoisonAmmo = 3;
    
    private int currentPoisonAmmo;
    private bool isRecharging = false;

    void Start()
    {
        currentPoisonAmmo = maxPoisonAmmo;
    }

void Update()
    {



        if(isRecharging)
        {
            return;
        }

        if (Input.GetButtonDown("Fire1"))
        {
            
            ReleasePoison();
            
        }
        if (currentPoisonAmmo <= 0)
        {
            StartCoroutine(ReloadPoison());
            return;
        }

        
        
    }

    IEnumerator ReloadPoison()
    {
        isRecharging = true;

        Debug.Log("Reloading Poison");

        yield return new WaitForSeconds(cooldownTime);

        currentPoisonAmmo = maxPoisonAmmo;

        isRecharging = false;




        

    }

    void ReleasePoison()
    {
        
       currentPoisonAmmo--;

        var poison = Instantiate(PoisonArea, poisonSpawnPoint.position, poisonSpawnPoint.rotation);
        

    }


}
