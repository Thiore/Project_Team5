using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using Unity.VisualScripting;

public enum Colors
{
    Red,
    Blue,
    Gray,
    Pink,
    Orange,
    Yellow,
    Sky_Blue,
}

public class Mission : MonoBehaviour, IPointerDownHandler, IPointerMoveHandler, IPointerUpHandler
{
    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("드래그 시작!");
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        Debug.Log("드래그 중!");
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log("드래그 종료!");
    }
}