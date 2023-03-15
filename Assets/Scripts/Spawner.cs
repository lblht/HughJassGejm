using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private float delay;
    [SerializeField] private GameObject prefab;
    [SerializeField] private GameObject wound;
    [SerializeField] private Transform targetTransform;

    private float timer;

    void FixedUpdate()
    {
        if(Time.time > timer && wound != null)
        {
            GameObject newPlatelet = Instantiate(prefab, transform.position, Quaternion.identity);
            newPlatelet.GetComponent<PlateletAI>().SetTargetTransform(targetTransform);
            timer = Time.time + delay;
        }

    }
}
