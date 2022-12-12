using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodCurrent : MonoBehaviour
{
    [SerializeField] ThirdPersonController thirdPersonController;
    [SerializeField] Animator animator;
    private bool inTunnel;
    private Collider bloodCurrent;

    void Update()
    {
        if(Input.GetButton("Jump") && inTunnel)
        {
            thirdPersonController.enabled = false;
            transform.position += bloodCurrent.transform.forward * Time.deltaTime * 20;
            animator.SetBool("flying", true);
            Vector3 newDirection = Vector3.RotateTowards(transform.forward, bloodCurrent.transform.forward, 0.1f, 0.0f);
            transform.rotation = Quaternion.LookRotation(newDirection);
        }
        else
        {
            thirdPersonController.enabled = true;
            animator.SetBool("flying", false);
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
