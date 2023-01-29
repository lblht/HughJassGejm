using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager instance;

    [SerializeField] GameObject tutorialScreen;

    private bool active = false;

    void Start()
    {
        tutorialScreen.SetActive(true);
        active = true;
        GameManager.instance.PauseGame();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T) && !active)
        {
            tutorialScreen.SetActive(true);
            active = true;
            GameManager.instance.PauseGame();
        }
        else if (Input.GetKeyDown(KeyCode.T) && active)
        {
            tutorialScreen.SetActive(false);
            active = false;
            GameManager.instance.UnPauseGame();
        }
    }
}
