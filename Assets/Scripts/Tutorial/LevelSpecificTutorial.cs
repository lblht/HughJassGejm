using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSpecificTutorial : MonoBehaviour
{
    [SerializeField] GameObject tutorialScreen;

    private bool active = false;

    void Start()
    {
        tutorialScreen.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T) && !active)
        {
            tutorialScreen.SetActive(true);
            active = true;
            GameManager.instance.PauseGame("Tutorial");
        }
        else if (Input.GetKeyDown(KeyCode.T) && active)
        {
            tutorialScreen.SetActive(false);
            active = false;
            GameManager.instance.UnPauseGame("Tutorial");
        }
    }
}
