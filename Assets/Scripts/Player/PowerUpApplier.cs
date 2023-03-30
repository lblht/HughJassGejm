using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerUpApplier : MonoBehaviour
{
    [SerializeField] private ThirdPersonController thirdPersonController; // referencia na ThirdPersonController skript postavy
    [SerializeField] private Image speedTimerUI; // UI element ktorý zobrazuje zostávajúci čas trvania speed boostu

    [SerializeField] private ParticleSystem dashEffect; // efekt spustenia boostu dash
    [SerializeField] private GameObject dashUI; // UI element ktorý zobrazuje že hráč môže použiť dash

    [SerializeField] private AudioPlayer audioPlayer;

    private float speedAdder = 4f; // hodnota o koľko sa zvýši rýchlosť postavy
    private float speedCooldown = 5f; 
    private float originalSpeed;    // originálne rýchlosť na ktorú sa po vypršaní cooldownu vráti
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
            audioPlayer.PlaySound("dash");
        }
    }

    public void SpeedBoost()
    {
        audioPlayer.PlaySound("powerUp");
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
        audioPlayer.PlaySound("powerUp");
        dash = true;
        dashUI.SetActive(true);
    }
}
