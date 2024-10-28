using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.EventSystems;

public class Wiring : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    eColor color;

    public eColor GetWiringColor()
    {
        return color;
    }

    public void SetWiringColor(int color)
    {
        this.color = (eColor)color;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log(color);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if(eventData.pointerEnter.gameObject.name == "EndPoint" && eventData.pointerEnter.gameObject.TryGetComponent(out WiringPoint points))
        {
            if(this.color == points.GetWiringColor())
            {
                Debug.Log("»ö±ò °°À½");
            }
            else
            {
                Debug.Log("»ö±ò Æ²¸²");
            }
        }
    }
}
