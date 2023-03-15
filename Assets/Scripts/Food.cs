using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
    [SerializeField] private float amount;  
    [SerializeField] private Renderer renderer;  

    private bool occupied;

    void Start()
    {
        GameManager.instance?.AddRecource();
    }

    public bool ReduceAmount(int value)
    {
        if(amount > 0)
        {
            amount -= value;

            if(amount <= 0)
            {
                gameObject.layer = LayerMask.NameToLayer("Default");   
                renderer.material.color = Color.black;
                GameManager.instance?.RemoveRecource();
            }

            return true;
        }
        else
        {
            return false;
        }
    }

    public void SetOccupied(bool value)
    {
        occupied = value;
        if(occupied)
            gameObject.layer = LayerMask.NameToLayer("Default");
        else
            gameObject.layer = LayerMask.NameToLayer("Food");
    }

    public bool GetOccupied()
    {
        return occupied;
    }
}
