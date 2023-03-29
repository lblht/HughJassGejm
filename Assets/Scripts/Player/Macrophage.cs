using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Macrophage : MonoBehaviour
{
    [SerializeField] private ThirdPersonController thirdPersonController;     
    [SerializeField] private LayerMask bacteriaLayer;  
    [SerializeField] private GameObject suckEffect;  
    [SerializeField] private Slider slider;  
    [SerializeField] private AudioPlayer audioPlayer;  

    private bool canEat = true;
    private bool eating;
    public float stamina = 100;

    void Update()
    {
        if(thirdPersonController.GetAllowControl())
            GetInput();
    }

    void FixedUpdate()
    {
        if(eating) 
            Eating();

        suckEffect.SetActive(eating);
    }

    void GetInput()
    {
        if((Input.GetKeyDown(KeyCode.E) || Input.GetMouseButtonDown(0)) && canEat)
        {
            eating = true;
            if(GameManager.instance && !GameManager.instance.GetPause())
                audioPlayer.PlaySound("suck");
        }
        if(Input.GetKeyUp(KeyCode.E) || Input.GetMouseButtonUp(0))
        {
            audioPlayer.StopSound("suck");
            eating = false;
        }

        if(eating && !canEat)
            eating = false;
    }

    void Eating()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position + (transform.forward * 2f) + (transform.up * - 1f), 2f, bacteriaLayer);

        foreach (var hitCollider in hitColliders)
        {
            hitCollider.gameObject.GetComponent<BacteriaAI>().Ragdoll(true, 5);
            hitCollider.gameObject.GetComponent<Rigidbody>().AddForce(Vector3.up * 5);
            hitCollider.gameObject.GetComponent<Rigidbody>().AddForce((transform.position - hitCollider.transform.position).normalized * 10);
            if(Vector3.Distance(transform.position, hitCollider.transform.position) < 1.8f)
            {
                hitCollider.gameObject.GetComponent<BacteriaDeath>().Die("Macrophage");
                StartCoroutine("Expand");
                audioPlayer.PlaySound("eat");
            }
        }
    }

    IEnumerator Expand()
    {
        bool expanding = true;
        bool a = true;
        float size = 1;
        float rate = 2f;

        while(a & expanding)
        {
            size += rate * Time.deltaTime;
            transform.localScale = new Vector3(size, transform.localScale.y, size);
            if(size >= 1.5f)
                a = false;
            yield return null; 
        }

        while(!a & expanding)
        {
            size -= rate * Time.deltaTime;
            transform.localScale = new Vector3(size, transform.localScale.y, size);
            if(size <= 1)
                expanding = false;
           yield return null; 
        }

        yield return null; 
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + (transform.forward * 2f) + (transform.up * - 1f), 2f);
    }
}
