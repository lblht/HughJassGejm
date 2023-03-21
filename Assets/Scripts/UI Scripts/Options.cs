using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;
using UnityEngine.Audio;

public class Options : MonoBehaviour
{
    public AudioMixer audioMixer;

    private bool tutorial = true;
    
    public void ChangeLanguage(int langID)
    {
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[langID];
    }

    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("MainVolume", volume/2);
    }
    
    public void SetTutorial(bool value)
    {
        tutorial = value;
    }

    public bool GetTutorial()
    {
        return tutorial;
    }
}
