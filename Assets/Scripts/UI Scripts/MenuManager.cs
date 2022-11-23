using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{

    [SerializeField] private GameObject[] activeAtStart;
    [SerializeField] private GameObject[] disabledAtStart;

    void Awake()
    {
        foreach(GameObject active in activeAtStart)
        {
            active.SetActive(true);
        }

        foreach(GameObject disabled in disabledAtStart)
        {
            disabled.SetActive(false);
        }
    }
}
