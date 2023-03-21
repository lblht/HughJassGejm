using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            gameObject.SetActive(false);
            GameManager.instance.HideCursor(true);
        }
    }

    void OnEnable()
    {
        GameManager.instance?.HideCursor(false);
        GameManager.instance?.PauseGame("PauseMenu");
    }

    void OnDisable()
    {
        GameManager.instance?.UnPauseGame("PauseMenu");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
