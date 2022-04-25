using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public TokenManager TokenMangerScript;
    public DialogueManager DialogueManagerScript;
    public Animator TheAnimator;


    public GameObject TokenObject;
    public bool IsToken;
    public GameObject DialogueObject;
    public bool IsDialogue;
    private UITest _checkUI;
    [Header("Pause Menu")]
    public GameObject MainMenuObject;
    public bool UsingMainMenu;
    public GameObject KeyBindings;
    public GameObject Settings;
    public GameObject ReturnToHub;
    public GameObject ExitGame;


    private void Start()
    {
        IsToken = false;
        IsDialogue = false;
        _checkUI = Object.FindObjectOfType<UITest>();
        UsingMainMenu = false;
    }
    void Update()
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

    public void ActivateKeyBindings()
    {
        KeyBindings.SetActive(true);
        Settings.SetActive(false);
        ReturnToHub.SetActive(false);
        ExitGame.SetActive(false);

    }
    public void ActivateSettings()
    {
        KeyBindings.SetActive(false);
        Settings.SetActive(true);
        ReturnToHub.SetActive(false);
        ExitGame.SetActive(false);
    }
    public void ActivateReturnToHub()
    {
        KeyBindings.SetActive(false);
        Settings.SetActive(false);
        ReturnToHub.SetActive(true);
        ExitGame.SetActive(false);
    }
    public void ActivateExitGame()
    {
        KeyBindings.SetActive(false);
        Settings.SetActive(false);
        ReturnToHub.SetActive(false);
        ExitGame.SetActive(true);
    }

    public void ReturnToTheHub()
    {
        SceneManager.LoadScene("The_Hub");
        UsingMainMenu = false;
    }
    public void ExitToDesktop()
    {
        SaveManager _saveManager = Object.FindObjectOfType<SaveManager>();
        _saveManager.Save();
        Application.Quit();
    }

}
