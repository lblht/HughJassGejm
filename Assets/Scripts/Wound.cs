using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wound : MonoBehaviour
{
    [SerializeField] private GameObject prefab;

    private float timer;
    private float timerDelay = 2;
    private float size = 1;

    void FixedUpdate()
    {
        if(Time.time > timer)
        {
            Vector3 spawnPosition = transform.position + new Vector3(Random.Range(-10f, 10f), -1f, Random.Range(-5f, 5f));
            GameObject bacteria = Instantiate(prefab, spawnPosition, Quaternion.identity);
            bacteria.GetComponent<BacteriaAI>().Ragdoll(true, 3f);
            bacteria.transform.rotation = Random.rotation;
            bacteria.GetComponent<Rigidbody>().AddTorque(50, 90, 230, ForceMode.Impulse);
            timer = Time.time + Random.Range(0, timerDelay);
        }

    }

    void OnTriggerEnter(Collider target)
    {
        if(target.tag == "Platelet")
        {
            Destroy(target.gameObject);
            size -= 0.01f;
            timerDelay += 0.5f;
            transform.localScale = new Vector3(size, size, transform.localScale.z);

            if(size < 0.05f)
                Destroy(gameObject);
        }
    }
}
