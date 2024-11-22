using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseDoor : MonoBehaviour,ITouchable
{
    [SerializeField] private int floorIndex;
    [SerializeField] private int objectIndex;

    public void OnTouchEnd(Vector2 position)
    {
        if (!SaveManager.Instance.PuzzleState(floorIndex, objectIndex))
        {
            DialogueManager.Instance.SetDialogue("Table_StoryB1", 1);
        }
    }

    public void OnTouchHold(Vector2 position)
    {
        
    }

    public void OnTouchStarted(Vector2 position)
    {
        
    }
}
