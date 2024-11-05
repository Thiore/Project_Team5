using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ReadInputData : MonoBehaviour
{
    public InputData data { private get;  set; }

    public Vector2 startValue { get; private set; }
    public Vector2 value { get; private set; }
    public bool isTouch;
    public float touchTime { get; private set; }

    

    private void Start()
    {
        data = null;
        startValue = transform.position;
        value = transform.position;
        isTouch = false;
        touchTime = 0f;
    }

    private void Update()
    {
        if(isTouch)
        {
            Performed();
        }
        
    }

    /// <summary>
    /// Click
    /// </summary>
    public void Started()
    {
        startValue = data.startValue;
        value = data.value;
        isTouch = data.isTouch;
        touchTime = 0f;
        Debug.Log("Started" + gameObject.name);
        Debug.Log(data.action);
    }
    /// <summary>
    /// Drag&Hold
    /// </summary>
    private void Performed()
    {
        Debug.Log("Performed" + gameObject.name);
        Debug.Log(gameObject.name+" : "+touchTime);
        value = data.value;
        touchTime += Time.unscaledDeltaTime;
        
        
    }
    /// <summary>
    /// Drop
    /// </summary>
    public void Ended()
    {
        Debug.Log("End" + gameObject.name);
        isTouch = false;
        touchTime = 0f;
    }

    public void TouchTap()
    {
        isTouch = false;
    }
}
