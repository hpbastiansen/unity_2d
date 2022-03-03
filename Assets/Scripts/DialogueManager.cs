using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{

    public Text nameText;
    public Text dialogueText;
    public Image img;
    public Animator animator;
    public bool ShowInteractButton;
    public GameObject EInteract;
    private Queue<string> sentences;
    public bool inDialogue;

    // Use this for initialization
    void Start()
    {
        sentences = new Queue<string>();
        animator.SetBool("IsOpen", false);

    }
    private void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            DisplayNextSentence();
        }
        if (ShowInteractButton)
        {
            EInteract.SetActive(true);
        }
        else
        {
            EInteract.SetActive(false);
        }
    }
    public void StartDialogue(Dialogue dialogue)
    {
        animator.SetBool("IsOpen", true);
        inDialogue = true;

        nameText.text = dialogue.name;
        img.sprite = dialogue.img;

        sentences.Clear();

        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(0.03f);
            yield return null;
        }
    }

    public void EndDialogue()
    {
        inDialogue = false;
        animator.SetBool("IsOpen", false);
    }

}