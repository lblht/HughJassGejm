using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Dialogue : MonoBehaviour
{
    public string dialogueName;
    public DialogueNodeScriptableObject[] dialogueNodes;
    public UnityEvent dialogueEndEvent;
}
