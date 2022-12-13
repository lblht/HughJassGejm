using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Powerups/SpeedBuff")]

public class SpeedBuff : PowerUpEffect
{

    public float amount = 1.1f;
    

    public override void Apply(GameObject target)
    {

        target.GetComponent<ThirdPersonController>().moveSpeed = target.GetComponent<ThirdPersonController>().moveSpeed * amount;
        target.GetComponent<ThirdPersonController>().sprintSpeed = target.GetComponent<ThirdPersonController>().sprintSpeed * amount;
    }




    
}



