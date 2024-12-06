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

    protected bool isInteracted = false;

    [SerializeField] protected TouchPuzzleCanvas puzzle;

    private void OnEnable()
    {
        if (objectIndex.Count.Equals(0))
        {
            isInteracted = true;
        }
        else
        {
            for (int i = objectIndex.Count - 1; i >= 0; --i)
            {
                if (!DataSaveManager.Instance.GetGameState(floorIndex, objectIndex[i]))
                {
                    isInteracted = false;
                    break;
                }
                else
                {
                    objectIndex.RemoveAt(i);
                }
                isInteracted = true;
            }
        }
    }
    public void InteractionObject(int obj)
    {
        objectIndex.Remove(obj);
        if (objectIndex.Count.Equals(0))
        {
            isInteracted = true;
        }
    }
}
