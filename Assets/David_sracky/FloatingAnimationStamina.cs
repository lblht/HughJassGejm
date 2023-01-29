using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingAnimationStamina : MonoBehaviour
{

    public float speed = 50;
    public float amplitude = 0.3f;
    public float frequency = 0.5f;
    Vector3 posOrigin = new Vector3();
    Vector3 tempPos = new Vector3();
    // Start is called before the first frame update
    void Start()
    {
        posOrigin = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        tempPos = posOrigin;
        tempPos.y += Mathf.Sin(Time.fixedTime * Mathf.PI * frequency) * amplitude;
        transform.position = tempPos;
        transform.Rotate(Vector3.forward * speed * Time.deltaTime);
        
    }
}
