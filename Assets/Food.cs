using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
    [SerializeField] private float amount;  
    [SerializeField] private Renderer renderer;  

    private bool occupied;

    public bool ReduceAmount(int value)
    {
        if((amount - value) >= 0)
        {
            amount -= value;
            return true;
        }
        else
        {
            gameObject.layer = LayerMask.NameToLayer("Default");   
            renderer.material.color = Color.black;
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
