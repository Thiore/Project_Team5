using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_InventorySlot : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private int id = -1;
    public int SlotID { get => id; }
    [SerializeField] private Item item;
    [SerializeField] private Image image;
    private bool isdragging = false;
    public void FragIsDrag()
    {
        isdragging = !isdragging;
    }

    public void SetinvenByID(Item item)
    {
        id = item.id;
        this.item = item;
        image.sprite = item.sprite;
    }

    public void SetInvenEmpty()
    {
        id = -1;
        item = null;
        image.sprite = null;
    }


    // ��, �ٿ� ���� �־�� ��� ��� ���� / eventsystem 50 ���� 
    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log("���Ӥ���");
        if (!isdragging)
        {
            UI_InvenManager.Instance.iteminfo.SetInfoByItem(item);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("�ٿ�");
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        isdragging = true;
        UI_InvenManager.Instance.dragimage.sprite = image.sprite;
        UI_InvenManager.Instance.dragimage.transform.position = eventData.position;
        UI_InvenManager.Instance.dragimage.gameObject.SetActive(true);
    }

    public void OnDrag(PointerEventData eventData)
    {
        // ���� ���δ� Infomation����, EndDrag �� PointerUp�̶� ���� �Ǽ� info �� OnDrop���� ó��
        UI_InvenManager.Instance.dragimage.transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        UI_InvenManager.Instance.dragimage.gameObject.SetActive(false);
        FragIsDrag();
    }
}
