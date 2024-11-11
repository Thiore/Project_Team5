using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverlapCheck : MonoBehaviour,ITouchable
{
    public List<OverlapPoint> pointList;

    private void OnEnable()
    {
        foreach(var point in pointList)
        {
            point.Check += CheckComplete;
        }
    }

    public void ResetBtnClick()
    {
        foreach(var point in pointList)
        {
            if (point.connectedObject.Count == 0) continue;
            
            foreach (var obj in point.connectedObject)
            {
                obj.isConnected = false;
                obj.line.enabled = false;
            }
            point.connectedObject.Clear();
        }
    }
    public void CheckComplete()
    {
        foreach(var point in pointList)
        {
            if (!point.GetComponent<OverlapObj>().isConnected)
            {
                Debug.Log("아직 게임 안끝남");
            }
        }
        Debug.Log("게임끝남");
    }

    public void OnTouchStarted(Vector2 position)
    {
        ResetBtnClick();
    }

    public void OnTouchHold(Vector2 position)
    {
    }

    public void OnTouchEnd(Vector2 position)
    {
    }
}
