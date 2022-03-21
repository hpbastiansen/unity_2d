using System.Collections;
using System.Collections.Generic;
using UnityEngine;


///The DialogueTrigger script is a script placed on an object in the world where one wish to start a new dialogue with the player.
public class DialogueTrigger : MonoBehaviour
{
    public Dialogue Dialogues;
    public bool AutoOpen;
    public GameObject EInteract;
    public bool CanStartDialogue;
    public DialogueManager DialogueManagerScript;

    /// Start methods run once when enabled.
    /**Start is called on the frame when a script is enabled just before any of the Update methods are called the first time.*/
    /**Firstly in the Start function CanStartDialogue is set to false, so that the player cannot start an empty dialogue, without it being triggered first.
    Then the DialogueManager script is located and assigned. */
    private void Start()
    {
        CanStartDialogue = false;
        DialogueManagerScript = GameObject.Find("Dialogue_Manager").GetComponent<DialogueManager>();
    }



    ///Update is called every frame.
    /**The Update function is FPS dependent, meaning it will update as often as it possibly can based on a change of frames. 
This means that is a game run on higher frames per second the update function will also be called more often.*/
    /*!The Update function checks if player can start dialogue, and if true and the player presses the interaction key, calls the TriggerDialogue function. */
    private void Update()
    {
        if (CanStartDialogue == true)
        {
            if (Input.GetKeyUp(KeyCode.E))
            {
                TriggerDialogue();
            }

        }
    }

    ///TriggerDialogue finds the DialogueManager script and calls StartDialogue.
    public void TriggerDialogue()
    {
        FindObjectOfType<DialogueManager>().StartDialogue(Dialogues);

    }

    ///TriggerDialogue finds the DialogueManager script and calls EndDialogue.
    public void EndDialogue()
    {
        FindObjectOfType<DialogueManager>().EndDialogue();

    }

    ///Sent each frame where another object is within a trigger collider attached to this object 
    /** Whenever another object with a given LayerMask has its 2D collider (with isTrigger enabled) inside this collider we check if the AutoOpen is false.
    If it is false and it is a gameObject with the layer "Player" we show the interaction button and allows the player to start the dialogue if wanted.*/
    private void OnTriggerStay2D(Collider2D other)
    {
        if (AutoOpen == false)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                DialogueManagerScript.ShowInteractButton = true;
                CanStartDialogue = true;
            }
        }
    }

    ///Sent when another object enters a trigger collider attached to this object
    /** Whenever another object with a given LayerMask has its 2D collider (with isTrigger enabled) inside this collider and if AutoOpen is true, the given dialogue is triggered.
    If the AutoOpen is false, we allow the player to start the dialogue.*/
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (AutoOpen == true)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                TriggerDialogue();
            }
        }
        else if (AutoOpen == false)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                DialogueManagerScript.ShowInteractButton = true;
                CanStartDialogue = true;
            }
        }
    }

    ///Sent when another object leaves a trigger collider attached to this object
    /** Whenever another object with a given LayerMask has its 2D collider (with isTrigger enabled) inside this collider we call EndDialogue to stop all dialogues,
    and disables the ability to start a dialogue until triggered again.*/
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            EndDialogue();
            DialogueManagerScript.ShowInteractButton = false;
            CanStartDialogue = false;
        }
    }
}