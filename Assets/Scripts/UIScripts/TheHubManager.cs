using UnityEngine;
using UnityEngine.SceneManagement;

/// This script manages the "The Hub" scene, opening the tutorial door if the player has completed the tutorial.
public class TheHubManager : MonoBehaviour
{
    public bool IsTutorialDone;
    public GameObject Door;

    /// Called before the first frame.
    private void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    /// Called every frame.
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

    /// Callback called after the scene has loaded.
    /** Saves the tutorial state to the save manager. */
    public void OnSceneLoaded(Scene _scene, LoadSceneMode _mode)
    {
        ES3.Save("Tutor", IsTutorialDone);
        IsTutorialDone = ES3.Load("Tutor", false);
    }
}
