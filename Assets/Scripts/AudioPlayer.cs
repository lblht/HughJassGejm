using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioPlayer : MonoBehaviour
{
    public AudioMixerGroup audioMixerGroup;
    public SoundClip[] soundClips;

    void Awake()
    {
        foreach(SoundClip soundClip in soundClips)
        {
            soundClip.audioSource = gameObject.AddComponent<AudioSource>();
            soundClip.audioSource.clip = soundClip.clip;
            soundClip.audioSource.volume = soundClip.volume;
            soundClip.audioSource.outputAudioMixerGroup = audioMixerGroup;
            soundClip.audioSource.playOnAwake = false;
        }
    }

    public void PlaySound(string clipName)
    {
        bool soundFound = false;

        foreach(SoundClip soundClip in soundClips)
        {
            if(soundClip.clipName == clipName)
            {
                if(soundClip.audioSource != null)
                {
                    soundClip.audioSource.Play();
                }
                soundFound = true;
            }
        }

        if(!soundFound)
            Debug.Log("Sound Clip Not Found");
    }

    public void StopSound(string clipName)
    {
        bool soundFound = false;

        foreach(SoundClip soundClip in soundClips)
        {
            if(soundClip.clipName == clipName)
            {
                soundClip.audioSource.Stop();
                soundFound = true;
            }
        }

        if(!soundFound)
            Debug.Log("Sound Clip Not Found");
    }
}
