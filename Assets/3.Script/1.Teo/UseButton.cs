using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class UseButton : MonoBehaviour, IUITouchable
{
    [SerializeField] private GameObject openObj;
    private GameObject currentObj;

    public void OnUIEnd(PointerEventData data)
    {
        if(currentObj == EventSystem.current.currentSelectedGameObject)
            openObj.SetActive(true);

        currentObj = null;
    }

    public void OnUIHold(PointerEventData data)
    {
        
    }

    public void OnUIStarted(PointerEventData data)
    {
        currentObj = EventSystem.current.currentSelectedGameObject;
    }
}
