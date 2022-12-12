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

    private bool canEat;
    private bool eating;
    private float stamina = 100;

    void Update()
    {
        GetInput();

        if(eating && stamina > 0) 
            Eating();
            
        CheckStamina();

        suckEffect.SetActive(eating);
    }

    void GetInput()
    {
        if(Input.GetKeyDown(KeyCode.E) && canEat)
            eating = true;
        if(Input.GetKeyUp(KeyCode.E))
            eating = false;

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
                AddStamina(5f);
                StartCoroutine("Expand");
            }
        }
    }

    void CheckStamina()
    {
        if(stamina < 100)
            AddStamina(Time.deltaTime * 2);

        if(thirdPersonController.IsSprinting() && thirdPersonController.GetMoveDirection().normalized != Vector3.zero)
            RemoveStamina(0.03f);

        if(eating)
            RemoveStamina(0.05f);

        if(stamina <= 0)
        {
            thirdPersonController.SetCanSprint(false);
            canEat = false;
        }
        else
        {
            thirdPersonController.SetCanSprint(true);
            canEat = true;
        }

        UpdateUI();
    }

    void AddStamina(float amount)
    {
        stamina += amount;
        
        if(stamina > 100)
            stamina = 100;
    }

    void RemoveStamina(float amount)
    {
        stamina -= amount;
        
        if(stamina < 0)
            stamina = 0;
    }

    void UpdateUI()
    {
       slider.value = stamina; 
    }

    IEnumerator Expand()
    {
        bool expanding = true;
        bool a = true;
        float size = 1;
        float rate = 0.02f;

        while(a & expanding)
        {
            size += rate;
            transform.localScale = new Vector3(size, transform.localScale.y, size);
            if(size >= 1.3f)
                a = false;
            yield return null; 
        }

        while(!a & expanding)
        {
            size -= rate;
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
