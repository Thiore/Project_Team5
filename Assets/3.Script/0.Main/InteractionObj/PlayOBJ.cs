using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayOBJ : MonoBehaviour
{
    [Header("SaveManager 참고")]
    [SerializeField] protected int floorIndex;
    public int getFloorIndex { get => floorIndex; }
    [SerializeField] protected List<int> objectIndex;
    public List<int> getObjectIndex { get => objectIndex; }

    [SerializeField] protected TouchPuzzleCanvas puzzle;
    
    public void InteractionObject(int obj)
    {
        objectIndex.Remove(obj);
    }
}
