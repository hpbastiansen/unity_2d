//https://youtu.be/_nRzoTzeyxU
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;


///The DialogueManager script is a script that only exist once in a scene. This script lets other scripts start a new dialogue, display next sentences, and end a dialogue.
public class DialogueManager : MonoBehaviour
{
    public Text NameText;
    public Text DialogueText;
    public Image ImageOfSpeaker;
    public Animator DialogueAnimator;
    public bool ShowInteractButton;
    public GameObject EInteract;
    private Queue<string> Sentences;
    public bool InDialogue;
    private UIManager _myUIManager;
    public bool RunFunctionAfter;
    public UnityEvent EventToRun;
    public AudioSource MyAudioSource;


    /// Start methods run once when enabled.
    /**Start is called on the frame when a script is enabled just before any of the Update methods are called the first time.*/
    /*! The Start function defines the "Sentences" varable to be a new Queue where it is a type of strings.
    We also makes sure the dialogues starts as "off" so that the player can see the dialouge box without content.*/
    void Start()
    {
        Sentences = new Queue<string>();
        RunFunctionAfter = false;
        //DialogueAnimator.SetBool("IsOpen", false);
        _myUIManager = GameObject.FindObjectOfType<UIManager>();

    }
    ///Update is called every frame.
    /**The Update function is FPS dependent, meaning it will update as often as it possibly can based on a change of frames. 
This means that is a game run on higher frames per second the update function will also be called more often.*/
    /*! The Update function allows the player to display the next sentences and shows the interaction button if the ShowInteractButtom boolean is true, and vice versa.*/
    private void Update()
    {
        if (Input.GetMouseButtonUp(1) && _myUIManager.IsDialogue && _myUIManager.UsingMainMenu == false)
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
        if (InDialogue)
        {
            _myUIManager.IsDialogue = true;
        }
        else
        {
            _myUIManager.IsDialogue = false;
        }
    }

    ///The StartDialogue function allows other scripts to start a new dialogue whenever conditions are met.
    /** Whenever the StartDialogue function is called and a dialogue object is set at its parameter the DialogueManager will start a new dialoge with that specified dialogue.
    The Animator component will show the dialogue, and change the variable values to whatever he dialogue specified.*/
    public void StartDialogue(Dialogue dialogue)
    {
        //DialogueAnimator.SetBool("IsOpen", true);
        InDialogue = true;

        NameText.text = dialogue.NameOfSpeaker;
        ImageOfSpeaker.sprite = dialogue.ImageOfSpeaker;

        Sentences.Clear();

        foreach (string sentence in dialogue.SentencesToSpeak)
        {
            Sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    ///The DisplayNextSentence function will show the next specified sentence and call the coroutine to type it out.
    public void DisplayNextSentence()
    {

        if (Sentences.Count == 0)
        {
            if (RunFunctionAfter)
            {
                EventToRun.Invoke();
            }
            EndDialogue();
            MyAudioSource.Stop();
            return;
        }
        MyAudioSource.Play();
        string sentence = Sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    ///The TypeSentence function will type out the setence letter by letter.
    /**When the TypeSentence funtion is called it will start by making sure the current text is set to blank ("").
    Then running through a foreach loop where it adds letter by letter to the sentence with a small delay between each letter.*/
    IEnumerator TypeSentence(string sentence)
    {
        DialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            DialogueText.text += letter;
            yield return new WaitForSeconds(0.03f);
            yield return null;
        }
        MyAudioSource.Stop();
    }

    ///The EndDialogue function lets other scripts end the dialogue.
    /**This function stops all current dialogue and removes the dialogue box. 
    This can be called e.g. when the end sentence has been read, or if the player chose to walk away from the dialogue area.*/
    public void EndDialogue()
    {
        MyAudioSource.Stop();
        InDialogue = false;
        DialogueAnimator.SetBool("IsOpen", false);
    }

}