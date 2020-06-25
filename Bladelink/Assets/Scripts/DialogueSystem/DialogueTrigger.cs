using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public DialogueObject dialogue;
    private bool wasTriggered;

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player") && !wasTriggered)
        {
            DialogueManager.Instance.StartDialogue(dialogue);
            wasTriggered = true;
        }
    }

}
