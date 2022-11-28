using UnityEngine;
using System.Collections;

public class Gun : MonoBehaviour
{
    public Transform bulletSpawnPoint;
    public GameObject bulletPrefab;
    public float bulletSpeed = 50;



    public float damage = 10f;
    public float range = 100f;

    public int maxAmmo=10;
    private int currentAmmo;
    public float reloadTime = 1f;
    private bool isReloading = false;



    public Camera fpsCam;

    void Start()
    {
        currentAmmo = maxAmmo;
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

        var bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
            bullet.GetComponent<Rigidbody>().velocity = bulletSpawnPoint.forward * bulletSpeed;



        // }



    }


}
