using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReadInputData : MonoBehaviour, ITouchable
{

    public Vector2 startValue { get; private set; }
    public Vector2 value { get; private set; }
    public bool isTouch;
    public float touchTime { get; private set; }

    

    private void Start()
    {
        startValue = transform.position;
        value = transform.position;
        isTouch = false;
        touchTime = 0f;
    }
    
    public void TouchTap()
    {
        isTouch = false;
    }
    /// <summary>
    /// Click
    /// </summary>
    public void OnTouchStarted(Vector2 position)
    {
        startValue = position;
        value = position;
        isTouch = true;
        touchTime = 0f;
    }
    /// <summary>
    /// Drag&Hold
    /// </summary>
    public void OnTouchHold(Vector2 position)
    {
        value = position;
        touchTime += Time.unscaledDeltaTime;
    }
    /// <summary>
    /// Drop
    /// </summary>
    public void OnTouchEnd(Vector2 position)
    {
        isTouch = false;
        touchTime = 0f;
    }
}
