using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayOBJ : MonoBehaviour
{
    [SerializeField] private TouchPuzzleCanvas puzzle;
    [SerializeField] private int interactionCount;
    public int getInteractionCount { get => interactionCount; }

    public void InteractionCount()
    {
        interactionCount -= 1;
        puzzle.SetQuickSlot();
    }
}
