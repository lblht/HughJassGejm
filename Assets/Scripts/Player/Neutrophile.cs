using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Neutrophile : MonoBehaviour
{
                                                    // len také info aby sme sa chápali, táto postava má 2 typy poisnu, taký ktorý položí na zem a tam zostane
                                                    // a taký ktorý aktívne vypúšťa
    [SerializeField] Transform poisonSpawnPoint;  // pozícia na ktorej sa bude spawnovať poison
    [SerializeField] GameObject poisonArea;       // prefab na objekt ktorý emituje poison v oblasti určitej veľkosti
    [SerializeField] GameObject poisonEmitArea;   // oblasť v ktorej poison ktorý hráč emituje bude dávať poškodenia
    [SerializeField] ParticleSystem poisonEmitEffect; // referencia na particle system effekt na vypúšťanie poisnu
    [SerializeField] float cooldownTime = 10;       // cooldown medzi tým kedy môžem znovu položiť area poison
    [SerializeField] int maxPoisonAmmo = 3;         
    [SerializeField] AudioPlayer audioPlayer;
    
    private bool poisonCharged = true;  // či hráč môže položiť poison


    void Update()
    {
        // ak hráč stlačí LMB postava začne vypúšťať poison
        if(Input.GetMouseButtonDown(0))
        {  
           poisonEmitArea.SetActive(true);
           poisonEmitEffect.Play();
        
            // ak hra nie je pauznuzá prehrá aj zvukový efekt
           if(GameManager.instance && !GameManager.instance.GetPause())
                audioPlayer.PlaySound("gas");
        }

        // ak hráš pustí LMB tak sa všetky tie veci ohladom vypúštania poisnu vypnú
        if(Input.GetMouseButtonUp(0))
        {  
           poisonEmitArea.SetActive(false);
           poisonEmitEffect.Stop();
           audioPlayer.StopSound("gas");
        }

        // ak hráč stlačí RMB alebo E a môže položiť poison (poisonCharged) 
        if((Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.E)) && poisonCharged)
        {  
            // tak zavolá funkciu na položenie poisnu
            ReleasePoison();  
            // a spustí coroutin na cooldown
            StartCoroutine(ReloadPoison());
        }
    }

    // co routine ktorá iba počíta cooldown kedy hráč môže znovu položiť poison
    IEnumerator ReloadPoison()
    {
        poisonCharged = false;

        Debug.Log("Reloading Poison");

        yield return new WaitForSeconds(cooldownTime);

        poisonCharged = true;
    }
    
    // funkcia spawne objekt poison area
    void ReleasePoison()
    {
        GameObject poison = Instantiate(poisonArea, poisonSpawnPoint.position, poisonSpawnPoint.rotation);
    }
}
