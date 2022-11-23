using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelectionScreen : MonoBehaviour
{
    [SerializeField] GameObject[] points;

    void Awake()
    {
        foreach(GameObject point in points)
        {
            point.SetActive(false);
        }
    }

    public void ShowPoints()
    {
        foreach(GameObject point in points)
        {
            point.SetActive(true);
        }
    }

    void OnDisable()
    {
        foreach(GameObject point in points)
        {
            point.SetActive(false);
        }
    }
}
