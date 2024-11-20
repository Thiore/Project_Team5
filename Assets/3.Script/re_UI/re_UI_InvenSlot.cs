using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class re_UI_InvenSlot : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IBeginDragHandler, IDragHandler
{
    [SerializeField] private int id = -1;
    public int SlotID { get => id; }
    [SerializeField] private re_Item item;
    [SerializeField] private Image image;
    private bool isdragging = false;
    public void FragIsDrag()
    {
        isdragging = !isdragging;
    }

    public void SetinvenByID(re_Item item)
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


    // 업, 다운 같이 있어야 기능 사용 가능 / eventsystem 50 설정 
    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log("더ㅣㅅ냐");
        if (!isdragging)
        {
            UI_InvenManager.Instance.iteminfo.SetInfoByItem(item);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("다운");  
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("비긴시작");
        isdragging = true;
        UI_InvenManager.Instance.dragimage.sprite = image.sprite;
        UI_InvenManager.Instance.dragimage.transform.position = eventData.position;
        UI_InvenManager.Instance.dragimage.gameObject.SetActive(true);
    }

    public void OnDrag(PointerEventData eventData)
    {
        // 조합 여부는 Infomation에서, EndDrag 시 PointerUp이랑 같이 되서 info 의 OnDrop에서 처리
        UI_InvenManager.Instance.dragimage.transform.position = eventData.position;
    }

}
