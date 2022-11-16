using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] private TextMeshProUGUI numberOfBacteriaTextUI;
    [SerializeField] private Slider slider;  

    private int numberOfBacteria;
    private int resourceAmount;

    void Awake()
    {
        instance = this;        // Toto bude robiť problemy možno treba to inak prešpekulovať
    }

    public void AddBacteria()
    {
        numberOfBacteria++;
        UpdateBacteriaUI();
    }

    public void RemoveBacteria()
    {
        numberOfBacteria--;
        UpdateBacteriaUI();
    }

    void UpdateBacteriaUI()
    {
        numberOfBacteriaTextUI.text = numberOfBacteria.ToString();
    }

    public void AddRecource()
    {
        resourceAmount++;
        slider.maxValue = resourceAmount;
        UpdateResourceUI();

    }

    public void RemoveRecource()
    {
        resourceAmount--;
        UpdateResourceUI();
    }

    void UpdateResourceUI()
    {
        slider.value = resourceAmount;
    }

    public void HideCursor(bool value)
    {
        if(value)
            Cursor.lockState = CursorLockMode.Locked;
        else
            Cursor.lockState = CursorLockMode.None;  
    }
}
