using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private DialogueManager dialogue;

    private void Start()
    {
        dialogue.SetDialogue("B1", 2);
    }
}
