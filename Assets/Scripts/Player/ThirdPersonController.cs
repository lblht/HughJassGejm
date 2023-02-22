using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ThirdPersonController : MonoBehaviour
{
    public CharacterController controller;    // referencia na character controller
    public Animator animator;   
    [SerializeField] private GameObject thirdPersonCameraPrefab;
    [SerializeField] public float moveSpeed;                   // rýchlosť chôdze
    [SerializeField] public float jumpHeight;                  // výška skoku
    [SerializeField] public float sprintSpeed;                 // rýchlosť šprintu
    [SerializeField] private LayerMask groundLayer;             // vrstva coliderov na ktorých môže chodiť
    [SerializeField] private GameObject speedEffect;   
    [SerializeField] private GameObject walkUI;   
    [SerializeField] private GameObject runUI;   

    private Transform camTransform;            // referencia na transform kamery   
    private GameObject thirdPersonCamera;

    private float horizontalInput;      // input z A,D / šípky v pravo, v ľavo
    private float verticalInput;        // input z W,S / šípky hore, dole
    private Vector3 inputDirection;     // smer pohybu určený horizontálnym a vertikálnym inputom
    private Vector3 moveDirection;

    private float turnSmoothTime = 0.2f;    // rýchlosť otáčania postavy do smeru pohybu
    private float turnSmoothVelocity;

    private Vector3 velocity;       // uchováva rýchlosť a smer skoku a pádu
    private float gravity = -9.8f;
    private float groundCheckOffset = 0.8f;                    // vrstva coliderov na ktorých môže chodiť
    private float groundCheckSize = 0.4f;                     // vrstva coliderov na ktorých môže chodiť
    private float speed;
    private bool sprinting = false;
    private bool canSprint = true; 

    void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        camTransform = Camera.main.transform;
        speed = moveSpeed;
        thirdPersonCamera = Instantiate(thirdPersonCameraPrefab, transform.position, Quaternion.identity);
        thirdPersonCamera.GetComponent<CinemachineFreeLook>().Follow = transform;
        thirdPersonCamera.GetComponent<CinemachineFreeLook>().LookAt = transform;
        animator.SetFloat("walkSpeedMultiplayer", moveSpeed/2);
        animator.SetFloat("runSpeedMultiplayer", sprintSpeed/5);
        ChangeSprintUI();
    }

    void OnDestroy()
    {
        Destroy(thirdPersonCamera, 5);
    }

    void Update()
    {
  
        GetInput();

        CalculateMovement();

        Gravity();
        ApplyMovement();

        if(IsGrounded())
            animator?.SetBool("isGrounded", true);
        else
            animator?.SetBool("isGrounded", false);

        if(IsGrounded() && inputDirection != Vector3.zero && !sprinting)
            animator?.SetBool("walking", true);
        else
            animator?.SetBool("walking", false);
        
        if(IsGrounded() && inputDirection != Vector3.zero && sprinting)
            animator?.SetBool("running", true);
        else
            animator?.SetBool("running", false);

        Dances();

        SpeedEffect();
    }

    void GetInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
        inputDirection = new Vector3(horizontalInput, 0f, verticalInput).normalized;

        if(Input.GetButtonDown("Jump") && IsGrounded())
        {
            Jump();
            animator?.SetTrigger("jump");
        }

        if(Input.GetKeyDown(KeyCode.LeftShift) && canSprint)
        {
            sprinting = !sprinting;
            ChangeSprintUI();
        }

        if(sprinting && !canSprint)
            sprinting = false;
    }

    void Gravity()
    {
        if(!IsGrounded())
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

            if(sprinting)
                speed = sprintSpeed;
            else
                speed = moveSpeed;
        }
        else
        {
            moveDirection = Vector3.zero;
        }
    }

    void ApplyMovement()
    {   
        controller.Move(moveDirection.normalized * speed * Time.deltaTime);    // horizontálny pohyb
        controller.Move(velocity * Time.deltaTime);                                // vertikálny pohyb
    }

    public void SetCanSprint(bool value)
    {
        canSprint = value;
    }

    public bool IsSprinting()
    {
        return sprinting;
    }

    public Vector3 GetMoveDirection()
    {
        return moveDirection;
    }

    void Dances()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
            animator?.Play("Dance1");
        if(Input.GetKeyDown(KeyCode.Alpha2))
            animator?.Play("Dance2");
        if(Input.GetKeyDown(KeyCode.Alpha3))
            animator?.Play("Dance3");
        if(Input.GetKeyDown(KeyCode.Alpha4))
            animator?.Play("Dance4");
        if(Input.GetKeyDown(KeyCode.Alpha5))
            animator?.Play("Dance5");
        if(Input.GetKeyDown(KeyCode.Alpha6))
            animator?.Play("Dance6");
        if(Input.GetKeyDown(KeyCode.Alpha7))
            animator?.Play("Dance7");
        if(Input.GetKeyDown(KeyCode.Alpha8))
            animator?.Play("Dance8");
    }

    void SpeedEffect()
    {
        if(speed > 8 && GetMoveDirection().normalized != Vector3.zero)
            speedEffect.SetActive(true);
        else
            speedEffect.SetActive(false);
    }

    public bool IsGrounded()
    {
        return Physics.CheckSphere(transform.position - new Vector3(0f, groundCheckOffset, 0f), groundCheckSize, groundLayer);
    }

    void ChangeSprintUI()
    {
        if(sprinting)
        {
            runUI.SetActive(true);
            walkUI.SetActive(false);
        }

        if(!sprinting)
        {
            runUI.SetActive(false);
            walkUI.SetActive(true);
        }
    }

    //debug
    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position - new Vector3(0f, groundCheckOffset, 0f), groundCheckSize);
    }
}
