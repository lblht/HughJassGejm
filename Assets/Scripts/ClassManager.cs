using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;

public class ClassManager : MonoBehaviour
{
    [SerializeField] private Animator classSelectionAnimator;
    [SerializeField] private GameObject selectionCamera;
    [SerializeField] private GameObject class1Prefab;
    [SerializeField] private GameObject class2Prefab;
    [SerializeField] private GameObject class3Prefab;
    [SerializeField] private GameObject class4Prefab;
    [SerializeField] private ClassCard classCard1;
    [SerializeField] private ClassCard classCard2;
    [SerializeField] private ClassCard classCard3;
    [SerializeField] private ClassCard classCard4;
    [SerializeField] private GameObject newClassScreen;
    [SerializeField] private Image newClassImage;

    private bool selectingClass;
    private bool selectingLocation;
    private GameObject selectedClass;
    private ClassCard currentClassCard;

    void Start()
    {
        newClassScreen.SetActive(false);
        currentClassCard = classCard1;
        classCard1.Locked(false);
        classCard2.Locked(false);
        classCard3.Locked(false);
        classCard4.Locked(false);
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab) && !selectingClass && !selectingLocation && !GameManager.instance.GetPause())
            OpenClassSelectionScreen();
        else if(Input.GetKeyDown(KeyCode.Tab) && selectingClass && !GameManager.instance.GetPause())
            CloseClassSelectionScreen();

        if(selectingLocation && Input.GetMouseButtonDown(0))
            SelectSpawnLocation();
    }

    void OpenClassSelectionScreen()
    {
        selectingClass = true;
        classSelectionAnimator.SetBool("Open", true);
        GameManager.instance.HideCursor(false);
        Time.timeScale = 0;
    }

    void CloseClassSelectionScreen()
    {
        selectingClass = false;
        classSelectionAnimator.SetBool("Open", false);
        GameManager.instance.HideCursor(true);
        Time.timeScale = 1;
    }

    void SelectSpawnLocation()
    {
        RaycastHit hit;
        Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit);
        GameManager.instance.HideCursor(true);
        Instantiate(selectedClass, hit.point + Vector3.up * 1f, Quaternion.identity);
        selectingLocation = false;
        selectionCamera.SetActive(false);
    }

    public void SelectClass(int classID)
    {
        selectingLocation = true;
        Time.timeScale = 1;
        selectingClass = false;
        selectionCamera.SetActive(true);
        classSelectionAnimator.SetBool("Open", false);
        Destroy(GameObject.FindGameObjectWithTag("Player"));
        currentClassCard.StartCooldown();
        
        switch (classID)                  
        {
        case 1:
            selectedClass = class1Prefab;
            currentClassCard = classCard1;
            break;
        case 2:
            selectedClass = class2Prefab;
            currentClassCard = classCard2;
            break;
        case 3:
            selectedClass = class3Prefab;
            currentClassCard = classCard3;
            break;
        case 4:
            selectedClass = class4Prefab;
            currentClassCard = classCard4;
            break;
        default:
            Debug.Log("Wrong Class ID!");
            break;
        }
    }

    public void UnlockClass(int classID)
    {
        switch (classID)                  
        {
        case 1:
            UnlockCard(classCard1);
            break;
        case 2:
            UnlockCard(classCard2);
            break;
        case 3:
            UnlockCard(classCard3);
            break;
        case 4:
            UnlockCard(classCard4);
            break;
        default:
            Debug.Log("Wrong Class ID!");
            break;
        }
    }

    void UnlockCard(ClassCard classCard)
    {
        classCard.Locked(false);
        newClassImage.sprite = classCard.GetCardSprite();
        newClassScreen.SetActive(true);
        GameManager.instance.HideCursor(false);
        GameManager.instance.PauseGame();
    }
}
