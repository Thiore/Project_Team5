using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class UseButton : MonoBehaviour, IUITouchable
{
    [SerializeField] private GameObject openObj;

    public void OnUIEnd(PointerEventData data)
    {
        
    }

    public void OnUIHold(PointerEventData data)
    {

    }

    public void OnUIStarted(PointerEventData data)
    {
        openObj.SetActive(true);
    }
}
