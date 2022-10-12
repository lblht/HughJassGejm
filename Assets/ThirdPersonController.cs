using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonController : MonoBehaviour
{
    [SerializeField] private CharacterController controller;    // referencia na character controller
    [SerializeField] private Transform camTransform;            // referencia na transform kamery   
    [SerializeField] private float moveSpeed;                   // rýchlosť chôdze
    [SerializeField] private float jumpHeight;                   // výška skoku
    [SerializeField] LayerMask groundLayer;                     // vrstva coliderov na ktorých môže chodiť

    private float horizontalInput;      // input z A,D / šípky v pravo, v ľavo
    private float verticalInput;        // input z W,S / šípky hore, dole
    private Vector3 inputDirection;     // smer pohybu určený horizontálnym a vertikálnym inputom
    private Vector3 moveDirection;

    private float turnSmoothTime = 0.2f;    // rýchlosť otáčania postavy do smeru pohybu
    private float turnSmoothVelocity;

    private Vector3 velocity;       // uchováva rýchlosť a smer skoku a pádu
    private float gravity = -9.8f;
    private float groundCheckOffset = 1f;                     // vrstva coliderov na ktorých môže chodiť
    private float groundCheckSize = 0.4f;                     // vrstva coliderov na ktorých môže chodiť
    private bool isGrounded;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;   // vypnutie zobrazovania kurzoru v hre
    }

    void Update()
    {
        isGrounded = Physics.CheckSphere(transform.position - new Vector3(0f, groundCheckOffset, 0f), groundCheckSize, groundLayer);
  
        GetInput();

        CalculateMovement();

        Gravity();
        ApplyMovement();
    }

    void GetInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
        inputDirection = new Vector3(horizontalInput, 0f, verticalInput).normalized;

        if(Input.GetButtonDown("Jump") && isGrounded)
            Jump();
    }

    void Gravity()
    {
        if(!isGrounded)
            velocity.y += gravity * Time.deltaTime;
        else if(velocity.y < 0)
            velocity.y = -2f;
    }

    void Jump()
    {
        velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
    }

    void CalculateMovement()
    {
        if(inputDirection.magnitude >= 0.1f)    // ak je nejaký horizontálny alebo vertikálny input
        {
            // vypožičaná matematika
            float targetAngle = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg + camTransform.eulerAngles.y;
            float angel = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angel, 0f);
            moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
        }
        else
        {
            moveDirection = Vector3.zero;
        }
    }

    void ApplyMovement()
    {   
        controller.Move(moveDirection.normalized * moveSpeed * Time.deltaTime);    // horizontálny pohyb
        controller.Move(velocity * Time.deltaTime);                                // vertikálny pohyb
    }


    //debug
    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position - new Vector3(0f, groundCheckOffset, 0f), groundCheckSize);
    }
}
