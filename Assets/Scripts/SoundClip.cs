using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SoundClip
{
    public string clipName;
    [Range(0f, 1f)]
    public float volume;
    public AudioClip clip;
    [HideInInspector]
    public AudioSource audioSource;
}
