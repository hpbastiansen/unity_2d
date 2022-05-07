using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TheHubManager : MonoBehaviour
{
    public bool IsTutorialDone;
    public GameObject Door;
    private void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;

    }
    public void Update()
    {
        if (IsTutorialDone)
        {
            Door.SetActive(false);
        }
        else
        {
            Door.SetActive(true);
        }
    }
    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        ES3.Save("Tutor", IsTutorialDone);
        IsTutorialDone = ES3.Load("Tutor", false);
    }
}
