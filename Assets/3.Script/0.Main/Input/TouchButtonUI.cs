using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class TouchButtonUI : MonoBehaviour,IUITouchable
{
    public UnityEvent onTouchStarted;
    public UnityEvent onTouchHold;
    public UnityEvent onTouchEnd;
    public void OnUIEnd(PointerEventData data)
    {
        onTouchEnd?.Invoke();
        
    }

    public void OnUIHold(PointerEventData data)
    {
        onTouchHold?.Invoke();
    }

    public void OnUIStarted(PointerEventData data)
    {
        onTouchStarted?.Invoke();
    }
}
