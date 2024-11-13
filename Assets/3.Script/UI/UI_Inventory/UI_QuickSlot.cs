using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_QuickSlot : MonoBehaviour, IEndDragHandler, IDragHandler, IBeginDragHandler, IPointerUpHandler
{
    [SerializeField] private Image dragImage;
    private Item copyItem;
    private Coroutine dragcoroutine;
    private float downTime;

    // 사용하면 떙겨지게 
    private void Awake()
    {

    }
    

    public void OnDrag(PointerEventData eventData)
    {
        dragImage.transform.position = eventData.position;
        Debug.Log("퀵 온 드래그");
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("퀵 포인트 다운");

        if (TryGetComponent(out copyItem))
        {
            if (copyItem.ID.Equals(2))
            {
                Debug.Log("되ㅣ,냐 ㄴㄻㄴㅇㅁㄹ");
            }
            else
            {
                dragImage.sprite = copyItem.Sprite;
                dragImage.transform.position = eventData.position;
                dragImage.gameObject.SetActive(true);
            }
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (copyItem.ID.Equals(2))
        {
            // 이걸로 큇슬롯 손전등 키기 끄기 
        }
        else
        {

        }

        if (dragImage.gameObject.activeSelf)
        {
            dragImage.gameObject.SetActive(false);
        }
        Debug.Log("퀵 드래그엔드");
        Debug.Log("여기다가 상호작용");
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (copyItem.ID.Equals(2))
        {
            // 이걸로 큇슬롯 손전등 키기 끄기 
        }
     
    }
}
