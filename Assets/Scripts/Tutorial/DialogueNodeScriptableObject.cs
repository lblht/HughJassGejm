using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DialogueNode", menuName = "ScriptableObjects/DialogueNode", order = 1)]
public class DialogueNodeScriptableObject : ScriptableObject
{
    [System.Serializable]
    public class Sentence
    {
        [TextArea(3,10)]
        public string sentence;
        public AudioClip audioClip;
    }

    public Sentence[] sentences;
}
