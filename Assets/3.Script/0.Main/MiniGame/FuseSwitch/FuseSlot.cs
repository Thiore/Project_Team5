using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FuseSlot : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private List<Fuse> fuselist;
    private GameObject ondragobj;

    public void OnBeginDrag(PointerEventData eventData)
    {
        for(int i = 0;  i < fuselist.Count; i++)
        {
            if (fuselist[i].gameObject.activeSelf)
            {
                fuselist[i].gameObject.transform.position = Camera.main.ScreenToViewportPoint(eventData.position);
                ondragobj = fuselist[i].gameObject;
                break;
            }
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        ondragobj.transform.position = Camera.main.ScreenToViewportPoint(eventData.position);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
       
    }

    public void OnPointerDown(PointerEventData eventData)
    {
       
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        
    }
}
