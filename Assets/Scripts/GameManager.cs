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
    [SerializeField] private GameObject youWinScreen; 
    [SerializeField] private ObjectiveManager objectiveManager; 
    [SerializeField] private GameObject qOn; 
    [SerializeField] private GameObject qPost; 

    private int numberOfBacteria;
    private int resourceAmount;
    private bool paused;
    private bool woundClosed;
    public bool infoHints = false;

    public int bacteriaKilledCount = 0;
    private string pauserId;

    void Start()
    {
        Application.targetFrameRate = 80;
        qOn.SetActive(false);
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

        if(Input.GetKeyDown(KeyCode.Q))
        {
            if(!infoHints)
            {
                infoHints = true;
                qOn.SetActive(true);
                HideCursor(false);
                PauseGame("Info");
                qPost?.SetActive(true);
            }
            else if(infoHints)
            {
                infoHints = false;
                qOn.SetActive(false);
                HideCursor(true);
                UnPauseGame("Info");
                qPost?.SetActive(false);
            }
            
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

        if(numberOfBacteria <= 0 && resourceAmount > 0 && woundClosed && responsible != "Mitosis")
        {
            youWinScreen.SetActive(true);

            //potom prerobit
            Destroy(youWinScreen, 10f);
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

    public void PauseGame(string id)
    {
        if(!GetPause())
        {
            Time.timeScale = 0;
            paused = true;
            pauserId = id;
        }
    }

    public void UnPauseGame(string id)
    {
        if(id == pauserId)
        {
            Time.timeScale = 1;
            paused = false;
        }
        else
            Debug.Log("Wrong Pauser ID");
    }

    public bool GetPause()
    {
        return paused;
    }

    public void WoundClosed()
    {
        woundClosed = true;

        if(numberOfBacteria <= 0 && resourceAmount > 0)
        {
            youWinScreen.SetActive(true);

            //potom prerobit
            Destroy(youWinScreen, 10f);
        }

    }
}
