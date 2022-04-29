using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

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
    // Start is called before the first frame update
    void Start()
    {
        _dialogueManagerScript = GameObject.Find("Dialogue_Manager").GetComponent<DialogueManager>();
        CanStartDialogue = false;
        IsOn = false;
    }

    // Update is called once per frame
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
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player") && IsOn == false && Puzzle1Script.GeneratorCanBeStarted == true)
        {
            _dialogueManagerScript.ShowInteractButton = true;
            CanStartDialogue = true;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            _dialogueManagerScript.ShowInteractButton = false;
            CanStartDialogue = false;
        }
    }

    IEnumerator StartTimer()
    {
        IsOn = true;
        yield return new WaitForSeconds(HowLongToStayOn);
        IsOn = false;
    }
}
