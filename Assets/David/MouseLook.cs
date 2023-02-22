using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{

    public float mouseSensitivity =100f;    //senzitivita na pozeranie

    public Transform playerBody;            //linkovanie objektu ?

    float xRotation =0f;




    // Start is called before the first frame update
    void Start()
    {

        Cursor.lockState = CursorLockMode.Locked;
        
    }

    // Update is called once per frame
    void Update()
    {
        //pozeranie z lava do prava 
        float MouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float MouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= MouseY;            //aby som sa nevedel pozriet za seba pri pozerani hore a dole
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);          


        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);      //pozeranie hore dole
        playerBody.Rotate(Vector3.up * MouseX);



    }
}
