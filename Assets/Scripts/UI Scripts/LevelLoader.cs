using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelLoader : MonoBehaviour
{
    [SerializeField] Options options;

    public void LoadLevel(string levelName)
    {
        if(levelName == "Tutorial" && options != null)
            if(options.GetTutorial())
                Application.LoadLevel("Tutorial");
            else
                Application.LoadLevel("LEVEL1");
        else
            Application.LoadLevel(levelName);
    }
}
