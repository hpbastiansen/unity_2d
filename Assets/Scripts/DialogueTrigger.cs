using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{

    public Dialogue dialogue;
    public bool autoOpen;
    public GameObject EInteract;
    public bool canStartDialogue;
    public DialogueManager dm;

    private void Start()
    {
        canStartDialogue = false;
        dm = GameObject.Find("Dialogue_Manager").GetComponent<DialogueManager>();

    }
    private void Update()
    {
        if (canStartDialogue == true)
        {
            if (Input.GetKeyUp(KeyCode.E))
            {
                TriggerDialogue();
            }

        }
    }
    public void TriggerDialogue()
    {
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue);

    }
    public void EndDialogue()
    {
        FindObjectOfType<DialogueManager>().EndDialogue();

    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (autoOpen == false)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                dm.ShowInteractButton = true;
                canStartDialogue = true;
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (autoOpen == true)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                TriggerDialogue();
            }
        }
        else if (autoOpen == false)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                dm.ShowInteractButton = true;
                canStartDialogue = true;
            }
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            EndDialogue();
            dm.ShowInteractButton = false;
            canStartDialogue = false;
        }
    }
}