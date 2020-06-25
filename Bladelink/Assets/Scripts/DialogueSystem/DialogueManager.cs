using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : Singleton<DialogueManager>
{
    public GameObject dialoguePanel;
    public GameObject optionsPanel;
    private DialogueObject currentDialogue;
    public Text dialogueText;
    public Text[] responseTexts;
    private Queue<string> sentences = new Queue<string>();
    
    private void Awake() {
        InputManager.controls.Player.ControlDialogue.performed += ctx => HandleInput();
    }

    void HandleInput() 
    {
        if(currentDialogue == null) return;
        if(sentences.Count == 0 && currentDialogue.responseOptions.Length <= 0) DisplayNextSentence();
        else if (sentences.Count != 0) DisplayNextSentence();
    }

    private void Start() {
        dialoguePanel.SetActive(false);
    }

    public void StartDialogue(DialogueObject dialogueObj)
    {
        dialoguePanel.SetActive(true);
        optionsPanel.SetActive(false);
        GameManager.Instance.hideHUD = true;
        currentDialogue = dialogueObj;
        sentences.Clear();

        Player.Instance.DisableControl(true);

        foreach (string sentence in dialogueObj.dialogue)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return null;
        }
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 1 && currentDialogue.responseOptions.Length > 0)
        {
            optionsPanel.SetActive(true);
            for (int i = 0; i < currentDialogue.responseOptions.Length; i++)
            {
                responseTexts[i].text = currentDialogue.responseOptions[i].reply;
            }
        } else if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }
    public void EndDialogue()
    {
        currentDialogue = null;
        dialoguePanel.SetActive(false);
        GameManager.Instance.hideHUD = false;
        Player.Instance.DisableControl(false);
    }

    public void TriggerResponse(int option)
    {
        Debug.Log("STARTED " + currentDialogue.responseOptions[option].dialogueObject.name);
        StartDialogue(currentDialogue.responseOptions[option].dialogueObject);
    }
}
