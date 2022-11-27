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
        [SerializeField] private int requiredKills;
        [SerializeField] private TextMeshProUGUI progressText;
        [SerializeField] private TextMeshProUGUI requiredText;
        [SerializeField] private GameObject objectiveUI;
        [SerializeField] private int classToUnlock;
        [HideInInspector] public ObjectiveManager objectiveManager;
        [HideInInspector] public bool active;

        private int currentKills;

        public void Initialize()
        {
            requiredText.text = "/" + requiredKills.ToString();
            Active(false);
        }

        public void UpdateObjective()
        {
            if(active)
            {
                currentKills++;
                progressText.text = currentKills.ToString();

                if(currentKills >= requiredKills)
                {
                    objectiveManager.classManager.UnlockClass(classToUnlock);
                    Active(false);
                    objectiveManager.NextObjective();
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
    {

    }

    [System.Serializable]
    public class DendriticCellObjective : ObjectiveTypeA
    {

    }

    public MacrophageObjective macrophageObjective;
    public DendriticCellObjective dendriticCellObjective;

    void Start()
    {
        macrophageObjective.Initialize();
        macrophageObjective.objectiveManager = this;

        dendriticCellObjective.Initialize();
        dendriticCellObjective.objectiveManager = this;

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
            dendriticCellObjective.Active(true);
            break;
        default:
            Debug.Log("No More Objectives");
            break;
        }
    }
}
