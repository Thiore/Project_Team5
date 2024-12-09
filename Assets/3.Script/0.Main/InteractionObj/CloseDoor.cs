using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseDoor : MonoBehaviour,ITouchable
{

    public void OnTouchEnd(Vector2 position)
    {
        DialogueManager.Instance.SetDialogue("Table_StoryB1", 1);
        
    }

    public void OnTouchHold(Vector2 position)
    {
        
    }

    public void OnTouchStarted(Vector2 position)
    {
        
    }
}
