using UnityEngine;
using System.Collections;
using TMPro;

public class Gun : MonoBehaviour
{
    public Transform bulletSpawnPoint;
    public GameObject bulletPrefab;
    public float bulletSpeed = 50;
    
    public WeaponSwitching weaponSwitching;

    public TextMeshProUGUI ammoDisplay;

    


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
        ammoDisplay.text = currentAmmo.ToString() + " / " + maxAmmo.ToString();
        Reload();
    }

    // Update is called once per frame
    void Update()
    {
        weaponSwitching.reloading = isReloading;

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
        ammoDisplay.text = currentAmmo.ToString() + " / " + maxAmmo.ToString();

    }



    void Shoot()
    {
        
        currentAmmo--;
        ammoDisplay.text = currentAmmo.ToString() + " / " + maxAmmo.ToString();
        


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
