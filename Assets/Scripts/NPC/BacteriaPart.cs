using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BacteriaPart : MonoBehaviour
{
    [SerializeField] private GameObject hintUI;
    [SerializeField] private Image circleUI;

    private bool playerInRange;
    private DendriticCell player;

    void Start()
    {
        hintUI.SetActive(false);
        circleUI.fillAmount = 0;
    }

    void Update()
    {
        if(playerInRange)
        {
            if(Input.GetKey(KeyCode.E))
            {
                circleUI.fillAmount += Time.deltaTime;

                if(circleUI.fillAmount >= 1)
                {
                    Destroy(gameObject);
                    GameManager.instance?.BacteriaPartCollected();
                }
            }
            else if(Input.GetKeyUp(KeyCode.E))
            {
                circleUI.fillAmount = 0;
            }

            hintUI.transform.position = Camera.main.WorldToScreenPoint(transform.position) + new Vector3(0f, 100f, 0f);
        }

    }

    void OnTriggerEnter(Collider target)
    {
        if(target.GetComponent<DendriticCell>() != null)
        {
            playerInRange = true;
            hintUI.SetActive(true);
            player = target.GetComponent<DendriticCell>();
        }
    }

    void OnTriggerExit(Collider target)
    {
        if(target.GetComponent<DendriticCell>() != null)
        {
            playerInRange = false;
            hintUI.SetActive(false);
            circleUI.fillAmount = 0;
        }
    }
}
