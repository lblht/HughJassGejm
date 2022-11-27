using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ClassCard : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private GameObject lockUI;
    [SerializeField] private float cooldown;
    [SerializeField] private TextMeshProUGUI cooldownText;
    [SerializeField] private GameObject cooldownUI;

    private float timer;

    void FixedUpdate()
    {
        if(timer > 0)
        {
            timer -= Time.deltaTime;
            cooldownText.text = timer.ToString("F0");
        }
        else
        {
            cooldownUI.SetActive(false);
            button.enabled = true;
        }
    }

    public void Locked(bool value)
    {
        button.enabled = !value;
        lockUI.SetActive(value);
    }

    public void StartCooldown()
    {
        timer = cooldown;
        cooldownUI.SetActive(true);
        button.enabled = false;
    }
}
