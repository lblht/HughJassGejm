using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BacteriaPart : MonoBehaviour
{
    [SerializeField] private int partID;
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
                    player.AddBacteriaPart(partID);
                }
            }
            else if(Input.GetKeyUp(KeyCode.E))
            {
                circleUI.fillAmount = 0;
            }
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
