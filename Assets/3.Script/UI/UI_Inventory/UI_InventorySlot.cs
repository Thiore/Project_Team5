using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_InventorySlot : MonoBehaviour, IEndDragHandler, IDragHandler, IBeginDragHandler
{
    [SerializeField] private Image dragImage;
    [SerializeField] private Image itemInformation;
    private Item copyItem;
    public Item getCopyItem { get => copyItem; }
    private Coroutine dragcoroutine;
    private float downTime;

    public void OnDrag(PointerEventData eventData)
    {
        dragImage.transform.position = eventData.position;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("아이템슬롯 포인트 다운");

        if (TryGetComponent(out copyItem))
        {
            dragImage.sprite = copyItem.Sprite;
            dragImage.transform.position = eventData.position;
            dragImage.gameObject.SetActive(true);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        dragImage.gameObject.SetActive(false);
        Debug.Log("드래그엔드");
        Debug.Log(eventData.pointerEnter.name);

        if (eventData.pointerEnter.TryGetComponent(out UI_ItemInformation info))
        {
            info.Combine(eventData);
        }

        //좀 개똥 처럼 해둠 


    }

}
