using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_InventorySlot : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    //private int id = -1;
    //public int SlotID { get => id; }
    public Item item { get; private set; }
    private int id;
    [SerializeField] private Image image;
    private bool isDragging = false;
    public void FragIsDrag()
    {
        isDragging = false;
    }

    public void SetinvenByItem(Item item)
    {
        this.item = item;
        id = item.id;
        image.sprite = item.sprite;
    }

    //public void SetInvenEmpty()
    //{
    //    id = -1;
    //    item = null;
    //    image.sprite = null;
    //}


    // ��, �ٿ� ���� �־�� ��� ��� ���� / eventsystem 50 ���� 
    public void OnPointerUp(PointerEventData eventData)
    {
        if (!isDragging)
        {
            UI_InvenManager.Instance.iteminfo.SetInfoByItem(item);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if(!isDragging)
        {
            isDragging = true;
            UI_InvenManager.Instance.dragImage.sprite = image.sprite;
            UI_InvenManager.Instance.dragImage.transform.position = eventData.position;
            UI_InvenManager.Instance.dragImage.gameObject.SetActive(true);
        }
            
    }

    public void OnDrag(PointerEventData eventData)
    {
        if(isDragging)
        {
            // ���� ���δ� Infomation����, EndDrag �� PointerUp�̶� ���� �Ǽ� info �� OnDrop���� ó��
            UI_InvenManager.Instance.dragImage.transform.position = eventData.position;
        }
       
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if(isDragging)
        {
            UI_InvenManager.Instance.dragImage.gameObject.SetActive(false);
            FragIsDrag();

        }
       
    }
}
