using UnityEngine;
using UnityEngine.SceneManagement;

/// This script is used to load the first scene of the game on startup.
public class LoadFirstScene : MonoBehaviour
{
    /// Called before the first frame.
    /** Load the 'The Hub' scene. */
    void Start()
    {
        SceneManager.LoadScene("The_Hub");
    }
}
