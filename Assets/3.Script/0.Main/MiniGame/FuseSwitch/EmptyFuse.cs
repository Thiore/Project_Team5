using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EmptyFuse : Fuse, ITouchable
{
    public void OnTouchEnd(Vector2 position)
    {
        Debug.Log("ㄹㄹ");
    }

    public void OnTouchHold(Vector2 position)
    {
   
    }

    public void OnTouchStarted(Vector2 position)
    {

    }
}



