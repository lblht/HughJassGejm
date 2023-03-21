using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Informational : MonoBehaviour
{
    [SerializeField] private GameObject hintUI;
    [SerializeField] private GameObject info;

    void Start()
    {
        hintUI.SetActive(false);
        info.SetActive(false);
    }

    void LateUpdate()
    {

        if(Input.GetKeyDown(KeyCode.Q))
        {
            Hints();
        }

        if(Input.GetMouseButton(0))
        {
            info.SetActive(false);
        }

        hintUI.transform.position = Camera.main.WorldToScreenPoint(transform.position);
        hintUI.transform.localScale =  new Vector3(12,12,12) / Vector3.Distance(Camera.main.transform.position, transform.position);
    }

    void OnDisable()
    {
        info.SetActive(false);
        hintUI.SetActive(false);
    }

    void Hints()
    {  
        info.SetActive(false);

        if(GameManager.instance.infoHints)
        {
            if(Vector3.Distance(Camera.main.transform.position, transform.position) < 25f && Vector3.Angle((transform.position - Camera.main.transform.position).normalized, Camera.main.transform.forward) < 90f)
            {
                hintUI.SetActive(true);
            }
        }
        else if(!GameManager.instance.infoHints)
        {
            hintUI.SetActive(false);
        }
    }

    public void ShowInformation()
    {
        info.SetActive(true);
    }
}
