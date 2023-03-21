using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings;

public class CutsceneTrigger : MonoBehaviour
{
    public Dialogue dialogueEN;
    public Dialogue dialogueSK;
    public DialogueManager dialogueManager;
    public GameObject cutsceneCamera;

    private bool done;

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player" && !done)
        {
            if(LocalizationSettings.SelectedLocale == LocalizationSettings.AvailableLocales.Locales[1])
                dialogueManager.StartDialogue(dialogueSK);
            else
                dialogueManager.StartDialogue(dialogueEN);

            dialogueManager.cutsceneCamera = cutsceneCamera;
            cutsceneCamera.SetActive(true);
            done = true;
        }
    }
}
