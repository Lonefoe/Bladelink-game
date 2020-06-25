using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ResponseOption
{
    [TextArea(2,2)]
    public string reply;
    public DialogueObject dialogueObject;

}
