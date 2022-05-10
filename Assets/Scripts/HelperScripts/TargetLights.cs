using System.Collections;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

/// This script is used for the light puzzle on stage 3. When the player interacts with the light, it turns on for an amount of time before disabling itself.
public class TargetLights : MonoBehaviour
{
    [Header("Local")]
    public bool IsOn;
    public SpriteRenderer MySpriteRenderer;
    public Sprite LightOff;
    public Sprite LightOn;
    public Light2D MyLight;
    private DialogueManager _dialogueManagerScript;
    public bool CanStartDialogue;
    public float HowLongToStayOn = 10;
    public Puzzle1 Puzzle1Script;

    /// Called before the first frame.
    /** Reset the light. */
    void Start()
    {
        _dialogueManagerScript = GameObject.Find("Dialogue_Manager").GetComponent<DialogueManager>();
        CanStartDialogue = false;
        IsOn = false;
    }

    /// Called every frame.
    /** If the light is turned on, change the sprite and light intensity. Listen for button press to turn on the light. */
    void Update()
    {
        if (IsOn == false)
        {
            MySpriteRenderer.sprite = LightOff;
            MyLight.intensity = 0;
        }
        else
        {
            MySpriteRenderer.sprite = LightOn;
            MyLight.intensity = 1;
            _dialogueManagerScript.ShowInteractButton = false;
        }
        if (CanStartDialogue && IsOn == false && Puzzle1Script.GeneratorCanBeStarted == true)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                StartCoroutine(StartTimer());
            }
        }
    }

    /// Called every frame a collider is inside the trigger on the gameobject.
    /** If the player is in the trigger, they can interact to start the light timer. */
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player") && IsOn == false && Puzzle1Script.GeneratorCanBeStarted == true)
        {
            _dialogueManagerScript.ShowInteractButton = true;
            CanStartDialogue = true;
        }
    }

    /// Called on a collider exiting the trigger on the gameobject.
    /** When exiting the trigger, the player can no longer interact with the light. */
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            _dialogueManagerScript.ShowInteractButton = false;
            CanStartDialogue = false;
        }
    }

    /// Coroutine enabling the light for an amount of time.
    IEnumerator StartTimer()
    {
        IsOn = true;
        yield return new WaitForSeconds(HowLongToStayOn);
        IsOn = false;
    }
}
