using UnityEngine;

public class WeaponSwitching : MonoBehaviour
{
    public int selectedWeapon = 0;

    public bool reloading = false;

    // Start is called before the first frame update
    void Start()
    {
        selectWeapon();
        

    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(GameManager.instance.bacteriaKilledCount);

        if (Input.GetKeyDown(KeyCode.Alpha1) && !reloading)
        {
            selectedWeapon=0;
            selectWeapon();
        }
        //&& GameManager.instance.bacteriaKilledCount >=1
        if (Input.GetKeyDown(KeyCode.Alpha2) && transform.childCount >=2 && !reloading)
        {
            selectedWeapon=1;
            selectWeapon();
        }
        if (Input.GetKeyDown(KeyCode.Alpha3) && transform.childCount >=3 && !reloading)
        {
            selectedWeapon=2;
            selectWeapon();
        }


       
    }

    void selectWeapon()
    {
        int i=0;
        foreach (Transform weapon in transform)
        {
            if (i==selectedWeapon)
            {
                weapon.gameObject.SetActive(true);
            }                
            else
            {
                weapon.gameObject.SetActive(false);
            }
            i++;

        }
        



    }
}
