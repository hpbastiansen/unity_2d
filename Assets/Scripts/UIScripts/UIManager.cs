using UnityEngine;
using UnityEngine.SceneManagement;

/// This script manages the Token, Dialogue and Pause Menu UI elements.
public class UIManager : MonoBehaviour
{
    public TokenManager TokenMangerScript;
    public DialogueManager DialogueManagerScript;
    public Animator TheAnimator;


    public GameObject TokenObject;
    public bool IsToken;
    public GameObject DialogueObject;
    public bool IsDialogue;

    [Header("Pause Menu")]
    public GameObject MainMenuObject;
    public bool UsingMainMenu;
    public GameObject KeyBindings;
    public GameObject Settings;
    public GameObject ReturnToHub;
    public GameObject ExitGame;

    /// Called before the first frame.
    private void Start()
    {
        IsToken = false;
        IsDialogue = false;
        UsingMainMenu = false;
    }

    /// Called every frame.
    /** The update function listens for changes in the Token and Dialogue Manager scripts or the Escape KeyDown event and shows the UI if applicable. */
    private void Update()
    {
        IsToken = TokenMangerScript.TokenUIactive;
        IsDialogue = DialogueManagerScript.InDialogue;

        if (IsDialogue || IsToken)
        {
            TheAnimator.SetBool("IsOpen", true);
        }
        else
        {
            TheAnimator.SetBool("IsOpen", false);
        }

        if (IsDialogue)
        {
            IsToken = false;
            TokenObject.SetActive(false);
            DialogueObject.SetActive(true);
            TokenMangerScript.TokenUIactive = false;
        }

        if (IsToken)
        {
            TokenMangerScript.TokenUIactive = true;
            IsDialogue = false;
            TokenObject.SetActive(true);
            DialogueObject.SetActive(false);

        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            UsingMainMenu = !UsingMainMenu;
            ActivateKeyBindings();
        }

        if (UsingMainMenu)
        {
            Cursor.lockState = CursorLockMode.None;
            MainMenuObject.SetActive(true);
            Time.timeScale = 0f;

        }
        else
        {
            Cursor.lockState = CursorLockMode.Confined;
            MainMenuObject.SetActive(false);
            Time.timeScale = 1f;
        }
    }

    /// Makes the pause menu show the "Key Bindings" screen.
    public void ActivateKeyBindings()
    {
        KeyBindings.SetActive(true);
        Settings.SetActive(false);
        ReturnToHub.SetActive(false);
        ExitGame.SetActive(false);

    }

    /// Makes the pause menu show the "Settings" screen.
    public void ActivateSettings()
    {
        KeyBindings.SetActive(false);
        Settings.SetActive(true);
        ReturnToHub.SetActive(false);
        ExitGame.SetActive(false);
    }

    /// Makes the pause menu show the "Return to Hub" screen.
    public void ActivateReturnToHub()
    {
        KeyBindings.SetActive(false);
        Settings.SetActive(false);
        ReturnToHub.SetActive(true);
        ExitGame.SetActive(false);
    }

    /// Makes the pause menu show the "Exit Game" screen.
    public void ActivateExitGame()
    {
        KeyBindings.SetActive(false);
        Settings.SetActive(false);
        ReturnToHub.SetActive(false);
        ExitGame.SetActive(true);
    }

    /// Sends the player back to the "The Hub" scene.
    public void ReturnToTheHub()
    {
        SceneManager.LoadScene("The_Hub");
        UsingMainMenu = false;
    }

    /// Saves and exits the game.
    public void ExitToDesktop()
    {
        SaveManager _saveManager = FindObjectOfType<SaveManager>();
        _saveManager.Save();
        Application.Quit();
    }

}
