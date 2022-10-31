using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Macrophage : MonoBehaviour
{
    [SerializeField] private LayerMask bacteriaLayer;  
    [SerializeField] private GameObject suckEffect;  

    private bool eating;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E))
            eating = true;
        if(Input.GetKeyUp(KeyCode.E))
            eating = false;

        suckEffect.SetActive(eating);

        if(eating)
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position + (transform.forward * 2f) + (transform.up * - 1f), 2f, bacteriaLayer);

            foreach (var hitCollider in hitColliders)
            {
                hitCollider.gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = false;
                hitCollider.gameObject.GetComponent<Rigidbody>().isKinematic = false;
                hitCollider.gameObject.GetComponent<Rigidbody>().useGravity = true;
                hitCollider.gameObject.GetComponent<Rigidbody>().AddForce(Vector3.up * 5);
                hitCollider.gameObject.GetComponent<Rigidbody>().AddForce((transform.position - hitCollider.transform.position).normalized * 10);
                if(Vector3.Distance(transform.position, hitCollider.transform.position) < 1f)
                    Destroy(hitCollider.gameObject);
            }

        }
    }


    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + (transform.forward * 2f) + (transform.up * - 1f), 2f);
    }
}
