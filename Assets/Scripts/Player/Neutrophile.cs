using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Neutrophile : MonoBehaviour
{
    [SerializeField] Transform poisonSpawnPoint;
    [SerializeField] GameObject poisonArea;
    [SerializeField] GameObject poisonEmitArea;
    [SerializeField] ParticleSystem poisonEmitEffect;
    [SerializeField] float cooldownTime = 10;
    [SerializeField] int maxPoisonAmmo = 3;
    [SerializeField] AudioPlayer audioPlayer;
    
    private bool poisonCharged = true;


    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {  
           poisonEmitArea.SetActive(true);
           poisonEmitEffect.Play();
           
           if(GameManager.instance && !GameManager.instance.GetPause())
                audioPlayer.PlaySound("gas");
        }

        if(Input.GetMouseButtonUp(0))
        {  
           poisonEmitArea.SetActive(false);
           poisonEmitEffect.Stop();
           audioPlayer.StopSound("gas");
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
        GameObject poison = Instantiate(poisonArea, poisonSpawnPoint.position, poisonSpawnPoint.rotation);
    }
}
