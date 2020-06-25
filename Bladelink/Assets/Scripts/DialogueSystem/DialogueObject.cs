using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="Dialogue",menuName="ScriptableObjects/Dialogues/New Dialogue")]
public class DialogueObject : ScriptableObject
{
    public string dialogueID;
    public Speaker speaker;

    [TextArea(3, 2)]
    public string[] dialogue;
    public ResponseOption[] responseOptions;
}

public enum Speaker
{
    Elder,
}
