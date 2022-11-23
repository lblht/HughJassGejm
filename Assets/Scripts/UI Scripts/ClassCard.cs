using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClassCard : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private GameObject lockUI;

    public void Locked(bool value)
    {
        button.enabled = !value;
        lockUI.SetActive(value);
    }
}
