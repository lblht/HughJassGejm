using UnityEngine;
using System.Collections;

public class RocketGun : MonoBehaviour
{
    public Transform rocketSpawnPoint;
    public Transform rocketRotation;

    public GameObject rocketPrefab;
    public float rocketSpeed = 50;



    public float damage = 10f;
    public float range = 100f;

    public int maxAmmo=1;
    private int currentAmmo;
    public float reloadTime = 2f;
    private bool isReloading = false;



    public Camera fpsCam;

    void Start()
    {
        currentAmmo = maxAmmo;
        Reload();
    }

    // Update is called once per frame
    void Update()
    {

        if(isReloading)
        {
            return;
        }

        if (Input.GetButtonDown("Fire1"))
        {
            
            Shoot();
        }
        if (currentAmmo <= 0)
        {
            StartCoroutine(Reload());
            return;
        }
        
    }

    IEnumerator Reload()
    {
        isReloading = true;

        Debug.Log("Reloading");

        yield return new WaitForSeconds(reloadTime);

        currentAmmo = maxAmmo;

        isReloading = false;

    }



    void Shoot()
    {
        
        currentAmmo--;


        // RaycastHit hit;
        // if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
        // {
        //     Debug.Log(hit.transform.name);

        //     Target target = hit.transform.GetComponent<Target>();
        //     if (target != null)
        //     { 
        //         target.TakeDamage(damage);
        //     }
        
         var rocket = Instantiate(rocketPrefab,rocketSpawnPoint.position, rocketRotation.rotation );
             rocket.GetComponent<Rigidbody>().velocity = rocketSpawnPoint.forward * rocketSpeed;

       


        // }



    }


}