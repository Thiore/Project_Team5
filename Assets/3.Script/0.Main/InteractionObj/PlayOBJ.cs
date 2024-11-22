using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayOBJ : MonoBehaviour
{
    [Header("SaveManager Âü°í")]
    [SerializeField] protected int floorIndex;
    public int getFloorIndex { get => floorIndex; }
    [SerializeField] protected int[] objectIndex;
    public int[] getObjectIndex { get => objectIndex; }

    [SerializeField] protected TouchPuzzleCanvas puzzle;
    [SerializeField] protected int interactionCount;
    public int getInteractionCount { get => interactionCount; }

    public void InteractionCount()
    {
        interactionCount -= 1;
        puzzle.SetQuickSlot();
    }
}
