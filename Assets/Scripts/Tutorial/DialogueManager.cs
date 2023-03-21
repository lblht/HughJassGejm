using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public GameObject dialogueBoxUI;
    public TextMeshProUGUI dialogueBoxTextUI;
    public Dialogue[] dialogues;
    public GameObject cutsceneCamera;
    public AudioPlayer audioPlayer;
    [SerializeField] ThirdPersonController thirdPersonController;

    private Queue<string> quedSentences;
    private Dialogue currentDialogue;
    private bool inDialogue;
    private int currentNodeIndex;

    void Start()
    {
        //currentDialogue = dialogues[0];

        quedSentences = new Queue<string>();
        dialogueBoxUI.SetActive(false);
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E) && inDialogue)
        {
            DisplayNextSentence();
        }
    }

    public void StartDialogue(Dialogue dialogue)
    {
        currentDialogue = dialogue;
        StartDialogueNode(0);
        DisplayNextSentence();
        dialogueBoxUI.SetActive(true);
        audioPlayer.PlaySound("open");
        thirdPersonController.SetAllowControl(false);
    }

    public void StartDialogueNode(int currentNodeIndex)
    {
        this.currentNodeIndex = currentNodeIndex;
        inDialogue = true;

        quedSentences.Clear();
        foreach (string sentence in currentDialogue.dialogueNodes[currentNodeIndex].sentences)
        {
            quedSentences.Enqueue(sentence);
        }
    }

    public void DisplayNextSentence()
    {
        if(quedSentences.Count == 0)
        {
            if(currentNodeIndex+1 < currentDialogue.dialogueNodes.Length)
            {
                StartDialogueNode(currentNodeIndex+1);
                DisplayNextSentence();
            }
            else
                EndDialogue();
        }
        else
        {
            string nextSentence = quedSentences.Dequeue();
            dialogueBoxTextUI.text = nextSentence;
            StopAllCoroutines();
            StartCoroutine(DisplaySentence(nextSentence));
        }
    }

    IEnumerator DisplaySentence(string sentence)
    {
        dialogueBoxTextUI.text = "";

        foreach(char letter in sentence.ToCharArray())
        {
            audioPlayer.PlaySound("letter");
            dialogueBoxTextUI.text += letter;
            yield return new WaitForSeconds(0.03f);
        }
    }

    void EndDialogue()
    {
        StopAllCoroutines();
        thirdPersonController.SetAllowControl(true);
        dialogueBoxUI.SetActive(false);
        inDialogue = false;
        cutsceneCamera.SetActive(false);
        currentDialogue = null;
    }
}
