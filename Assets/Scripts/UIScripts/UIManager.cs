using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private void Start()
    {
        IsToken = false;
        IsDialogue = false;
        _checkUI = Object.FindObjectOfType<UITest>();

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

    }
}
