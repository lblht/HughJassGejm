using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private float delay;
    [SerializeField] private GameObject prefab;

    private float timer;

    void FixedUpdate()
    {
        if(Time.time > timer)
        {
            Instantiate(prefab, transform.position, Quaternion.identity);
            timer = Time.time + delay;
        }

    }
}
