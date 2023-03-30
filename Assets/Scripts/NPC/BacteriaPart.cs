using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BacteriaPart : MonoBehaviour
{
    [SerializeField] private GameObject hintUI; // referencia na UI pomôcku ktorá hráčovi ukazuje akou klávesou má tento objekt zobrať
                                                // a náplň kruhu ktorý ukazuje čas ako dlho danú klávesu musí držať aby sa zobratie uskutočnilo
    [SerializeField] private Image circleUI; // kruh ktorý udáva hodnotu ako dlho je spomenutá klávesa držaná

    private bool playerInRange; // asi nemusím vysvetlovať
    private DendriticCell player; // do tejto premennej bude neskôr uložená referencia na skrip DendriticCell postavy za ktorú hráč momentálne hrá

    void Start()
    {
        hintUI.SetActive(false); // na začitaku sa UI pomôcka pre zdvihnutie objektu vypne
        circleUI.fillAmount = 0; // výplň kruhu sa nastaví na 0
    }

    void Update()
    {
        // ak je hráč v oblasti a E je stlačené kruh sa začne napĺňať
        if(playerInRange)
        {
            if(Input.GetKey(KeyCode.E))
            {
                circleUI.fillAmount += Time.deltaTime;


                // ak sa kruh naplní, skript dá game managerovi vedieť že hráč zdvihol tento objekt a zničí sa
                if(circleUI.fillAmount >= 1)
                {
                    GameManager.instance?.BacteriaPartCollected();
                    Destroy(gameObject);
                }
            }
            else if(Input.GetKeyUp(KeyCode.E)) // ak hráč prestane držať E tak sa výplň kruhu vrátu na 0
            {
                circleUI.fillAmount = 0;
            }

            // updatuje pozíciu UI pomôcky na obrazovke tak aby sa zobrazovala vždy nad týmto objektom
            hintUI.transform.position = Camera.main.WorldToScreenPoint(transform.position) + new Vector3(0f, 100f, 0f);
        }

    }


    // ak hráč vojde do oblasti (je dostatočne blízko na zdvihnutie) a je to DendriticCell nastavia sa potrebné premnné a zapne sa UI pomôcka
    void OnTriggerEnter(Collider target)
    {
        if(target.GetComponent<DendriticCell>() != null)
        {
            playerInRange = true;
            hintUI.SetActive(true);
            player = target.GetComponent<DendriticCell>();
        }
    }

    // ak hráč z danej oblasti odíde urobí sa v podstate opak
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
