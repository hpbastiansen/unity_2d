using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnEnableDialogueManager : MonoBehaviour
{
    public DialogueTrigger DialogueTriggerObject;
    public GameObject TriggerObject;
    public Dialogue Dialogues;
    public void ActivateDialogue()
    {
        DialogueTriggerObject.Dialogues.SentencesToSpeak.Clear();
        DialogueTriggerObject.Dialogues = Dialogues;
        DialogueTriggerObject.TriggerDialogue();
    }
}
