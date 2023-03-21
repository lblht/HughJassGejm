using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialEnd : MonoBehaviour
{
    [SerializeField] private Image circleUI;

    void Start()
    {
        circleUI.fillAmount = 0;
    }

    void Update()
    {
        if(Input.GetKey(KeyCode.F))
        {
            circleUI.fillAmount += Time.deltaTime;

            if(circleUI.fillAmount >= 1)
            {
                LoadLevel("LEVEL1");
            }
        }
        else if(Input.GetKeyUp(KeyCode.F))
        {
            circleUI.fillAmount = 0;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            LoadLevel("LEVEL1");
        }
    }

    void LoadLevel(string levelName)
    {
        Application.LoadLevel(levelName);
    }
}


