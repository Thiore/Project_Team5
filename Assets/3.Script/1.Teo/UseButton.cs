using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class UseButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private GameObject openObj;
    private GameObject currentObj;

    public void OnPointerDown(PointerEventData eventData)
    {
        currentObj = eventData.pointerEnter;
        
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if(gameObject.Equals(currentObj))
            openObj.SetActive(true);
    }
}
