using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Powerups/ResetStamina")]

public class ResetStamina : PowerUpEffect
{

    public float amount = 100;
    

    public override void Apply(GameObject target)
    {

        target.GetComponent<Macrophage>().stamina = amount;
     }




    
}



