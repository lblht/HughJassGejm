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
    [SerializeField] private GameObject pauseMenu; 
    [SerializeField] private GameObject gameOverScreen; 
    [SerializeField] private ObjectiveManager objectiveManager; 

    private int numberOfBacteria;
    private int resourceAmount;
    private bool paused;

    public int bacteriaKilledCount = 0;

    public bool prak=true;
    public bool luk=false;
    public bool bazuka=false;

    void Start()
    {
        Application.targetFrameRate = 80;
    }

    public void setBazuka (bool value)
    {
        bazuka = value;
    }
    public bool getBazuka()
    {
        return bazuka;
    }

    public void setLuk (bool value)
    {
        luk = value;
    }
    public bool getLuk()
    {
        return luk;
    }


    void Awake()
    {
        instance = this;        // Toto bude robiť problemy možno treba to inak prešpekulovať
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape) && !paused)
        {
            pauseMenu.SetActive(true);
        }
    }

    public void AddBacteria()
    {
        numberOfBacteria++;
        UpdateBacteriaUI();
    }

    public void RemoveBacteria(string responsible)
    {
        
        numberOfBacteria--;
        UpdateBacteriaUI();

        switch (responsible)                  
        {
        case "Macrophage":
            objectiveManager.macrophageObjective.UpdateObjective();
            break;
        case "Neutrophile":
            objectiveManager.neutrophileObjective.UpdateObjective();
            break;
        case "TCell":
            objectiveManager.tCellObjective.UpdateObjective();
            bacteriaKilledCount++;
            break;
        default:
            break;
        }
    }

    public void BacteriaPartCollected()
    {
        objectiveManager.dendriticCellObjective.UpdateObjective();
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

        if(resourceAmount <= 0)
        {
            gameOverScreen.SetActive(true);

            //potom prerobit
            Destroy(gameOverScreen, 10f);
        }
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

    public void PauseGame()
    {
        Time.timeScale = 0;
        paused = true;
    }

    public void UnPauseGame()
    {
        Time.timeScale = 1;
        paused = false;
    }

    public bool GetPause()
    {
        return paused;
    }
}
