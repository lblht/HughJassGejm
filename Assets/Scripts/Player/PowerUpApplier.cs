using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerUpApplier : MonoBehaviour
{
    [SerializeField] private ThirdPersonController thirdPersonController;
    [SerializeField] private Image speedTimerUI;

    [SerializeField] private ParticleSystem dashEffect;
    [SerializeField] private GameObject dashUI;

    private float speedAdder = 4f;
    private float speedCooldown = 5f;
    private float originalSpeed;
    private float speedTimer;
    private bool speedBoost;

    private bool dash;
    private float dashTimer;

    void Start()
    {
        speedTimerUI.fillAmount = 0;
        originalSpeed = thirdPersonController.sprintSpeed;
        dashUI.SetActive(false);
    }

    void FixedUpdate()
    {
        if(dashTimer > Time.time)
        {
            thirdPersonController.controller.Move(transform.forward * 2f);
        }
    }

    void Update()
    {
        if(speedTimer > Time.time && speedBoost)
        {
            speedTimerUI.fillAmount = (speedTimer - Time.time) / speedCooldown;
        }
        else if(speedBoost)
        {
            ResetSpeed();
        }

        if(Input.GetButtonDown("Jump") && thirdPersonController.enabled && !thirdPersonController.IsGrounded() && dash)
        {
            dashEffect.Play();
            dash = false;
            dashUI.SetActive(false);
            dashTimer = Time.time + 0.15f;
        }
    }

    public void SpeedBoost()
    {
        thirdPersonController.sprintSpeed += speedAdder;
        speedBoost = true;
        speedTimer = Time.time + speedCooldown;
        speedTimerUI.fillAmount = 1;
    }

    void ResetSpeed()
    {
        speedBoost = false;
        thirdPersonController.sprintSpeed = originalSpeed;
    }

    public void Dash()
    {
        dash = true;
        dashUI.SetActive(true);
    }
}
