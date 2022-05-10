using UnityEngine;

/// This script prepares the dialogue that is shown.
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
