using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelectionPoint : MonoBehaviour
{
    [SerializeField] private GameObject levelSelectionScreen;
    [SerializeField] private GameObject intro;
    [SerializeField] private Animator animator;

    void Start()
    {
        transform.localScale = Vector3.zero;
    }

    public void PlayIntro()
    {
        intro.SetActive(true);
        levelSelectionScreen.SetActive(false);
    }
}
