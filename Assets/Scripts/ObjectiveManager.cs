using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ObjectiveManager : MonoBehaviour
{
    [SerializeField] private ClassManager classManager;

    private int currentObjectiveID = 0;

    public class ObjectiveTypeA
    {
        [SerializeField] private int required;
        [SerializeField] private TextMeshProUGUI progressText;
        [SerializeField] private TextMeshProUGUI requiredText;
        [SerializeField] private GameObject objectiveUI;
        [SerializeField] private string objectiveID;
        [HideInInspector] public ObjectiveManager objectiveManager;
        [HideInInspector] public bool active;

        private int currentKills;

        public void Initialize()
        {
            requiredText.text = "/" + required.ToString();
            Active(false);
        }

        public void UpdateObjective()
        {
            if(active)
            {
                currentKills++;
                progressText.text = currentKills.ToString();

                if(currentKills >= required)
                {
                    objectiveManager.ObjectiveFinished(objectiveID);
                    Active(false);
                }
            }
        }

        public void Active(bool value)
        {
            objectiveUI.SetActive(value);
            active = value;
        }
    }

    [System.Serializable]
    public class MacrophageObjective : ObjectiveTypeA
    {}

    [System.Serializable]
    public class NeutrophileObjective : ObjectiveTypeA
    {}

    [System.Serializable]
    public class DendriticCellObjective : ObjectiveTypeA
    {}

    [System.Serializable]
    public class TCellObjective : ObjectiveTypeA
    {}

    public MacrophageObjective macrophageObjective;
    public NeutrophileObjective neutrophileObjective;
    public DendriticCellObjective dendriticCellObjective;
    public TCellObjective tCellObjective;

    void Start()
    {
        macrophageObjective.Initialize();
        macrophageObjective.objectiveManager = this;

        neutrophileObjective.Initialize();
        neutrophileObjective.objectiveManager = this;

        dendriticCellObjective.Initialize();
        dendriticCellObjective.objectiveManager = this;

        tCellObjective.Initialize();
        tCellObjective.objectiveManager = this;

        NextObjective();
    }

    public void ObjectiveFinished(string objectiveID)
    {
        switch (objectiveID)                  
        {
        case "MacrophageObjective":
            classManager.UnlockClass(2);
            break;
        case "NeutrophileObjective":            
            classManager.UnlockClass(3);
            break;
        case "DendriticCellObjective":
            classManager.UnlockClass(4);
            break;
        case "TCellObjective":
            // GameManager.instance.setBazuka(true);            
            break;
        default:
            Debug.Log("Unknown Objective ID");
            break;
        }

        NextObjective();
    }

    public void NextObjective()
    {
        currentObjectiveID++;

        switch (currentObjectiveID)                  
        {
        case 1:
            macrophageObjective.Active(true);
            break;
        case 2:
            neutrophileObjective.Active(true);
            break;
        case 3:
            dendriticCellObjective.Active(true);
            break;
        case 4:
            tCellObjective.Active(true);
            break;
        default:
            Debug.Log("No More Objectives");
            break;
        }
    }
}
