using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodCurrent : MonoBehaviour
{
    [SerializeField] ThirdPersonController thirdPersonController;
    [SerializeField] PlayerMovement firstPersonController;
    [SerializeField] Animator animator;
    [SerializeField] bool allowRotation;
    private bool inTunnel;
    private Collider bloodCurrent;

    void Update()
    {
        if(Input.GetButton("Jump") && inTunnel)
        {
            if(thirdPersonController != null)
            {
                thirdPersonController.enabled = false;
                animator.SetBool("flying", true);
            }

            if(firstPersonController != null)
                firstPersonController.enabled = false;

            transform.position += bloodCurrent.transform.forward * Time.deltaTime * 20;

            if(!allowRotation)
            {
                Vector3 newDirection = Vector3.RotateTowards(transform.forward, bloodCurrent.transform.forward, 0.1f, 0.0f);
                transform.rotation = Quaternion.LookRotation(newDirection);
            }
        }
        else
        {
            if(thirdPersonController != null)
            {
                thirdPersonController.enabled = true;
                animator.SetBool("flying", false);
            }
            if(firstPersonController != null)
                firstPersonController.enabled = true;
        }
    }

    void OnTriggerEnter(Collider target)
    {
        if(target.tag == "BloodCurrent")
        {
            inTunnel = true;
            bloodCurrent = target;
        }
    }

    void OnTriggerExit(Collider target)
    {
        if(target.tag == "BloodCurrent")
        {
            inTunnel = false;
            bloodCurrent = null;
        }
    }
}
