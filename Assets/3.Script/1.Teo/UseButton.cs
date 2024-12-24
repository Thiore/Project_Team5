using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;


public class UseButton : MonoBehaviour, IUITouchable
{
    public UnityEvent onTouchStarted;
    public UnityEvent OnTouchHold;
    public UnityEvent OnTouchEnd;
    public void OnUIEnd(PointerEventData data)
    {
        OnTouchEnd?.Invoke();
    }

    public void OnUIHold(PointerEventData data)
    {
        OnTouchHold?.Invoke();
    }

    public void OnUIStarted(PointerEventData data)
    {
        onTouchStarted?.Invoke();
    }
}
