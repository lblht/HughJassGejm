using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ClassManager : MonoBehaviour
{
    [SerializeField] private Animator classSelectionAnimator;
    [SerializeField] private CinemachineFreeLook thirdPersonCamera;
    [SerializeField] private GameObject selectionCamera;
    [SerializeField] private GameObject class1Prefab;
    [SerializeField] private GameObject class2Prefab;
    [SerializeField] private GameObject class3Prefab;
    [SerializeField] private GameObject class4Prefab;

    private bool selectingClass;
    private bool selectingLocation;
    private GameObject selectedClass;
    
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab) && !selectingClass && !selectingLocation)
            OpenClassSelectionScreen();
        else if(Input.GetKeyDown(KeyCode.Tab) && selectingClass)
            CloseClassSelectionScreen();

        if(selectingLocation && Input.GetMouseButtonDown(0))
            SelectSpawnLocation();
    }

    void OpenClassSelectionScreen()
    {
        selectingClass = true;
        classSelectionAnimator.Play("OpenClassSelection");
        GameManager.instance.HideCursor(false);
        Time.timeScale = 0;
    }

    void CloseClassSelectionScreen()
    {
        selectingClass = false;
        classSelectionAnimator.Play("CloseClassSelection");
        GameManager.instance.HideCursor(true);
        Time.timeScale = 1;
    }

    void SelectSpawnLocation()
    {
        RaycastHit hit;
        Physics.Raycast (Camera.main.ScreenPointToRay(Input.mousePosition), out hit);
        GameManager.instance.HideCursor(true);
        GameObject newClass = Instantiate(selectedClass, hit.point + Vector3.up * 1f, Quaternion.identity);
        selectingLocation = false;
        selectionCamera.SetActive(false);
        thirdPersonCamera.Follow = newClass.transform;
        thirdPersonCamera.LookAt = newClass.transform;
    }

    public void SelectClass(int classID)
    {
        selectingLocation = true;
        Time.timeScale = 1;
        selectingClass = false;
        selectionCamera.SetActive(true);
        classSelectionAnimator.Play("CloseClassSelection");
        Destroy(GameObject.FindGameObjectWithTag("Player"));
        
        switch (classID)                  
        {
        case 1:
            selectedClass = class1Prefab;
            break;
        case 2:
            selectedClass = class2Prefab;
            break;
        case 3:
            selectedClass = class3Prefab;
            break;
        case 4:
            selectedClass = class4Prefab;
            break;
        default:
            Debug.Log("Wrong Class ID!");
            break;
        }
    }
}
