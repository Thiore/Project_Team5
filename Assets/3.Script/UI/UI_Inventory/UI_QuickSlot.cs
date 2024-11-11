using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_QuickSlot : MonoBehaviour,IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    [SerializeField] private Image dragImage;
    private Item copyItem;
    private Coroutine dragcoroutine;
    private float downTime;

    private void Awake()
    {
        
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        dragcoroutine = StartCoroutine(HoldDragStart(eventData));

    }

    public void OnDrag(PointerEventData eventData)
    {
        dragImage.transform.position = eventData.position;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        // 마우스를 땔때 여기서 상호작용 메소드 넣으면 됨 
        dragImage.gameObject.SetActive(false);
    }

    private IEnumerator HoldDragStart(PointerEventData eventData)
    {
        while (downTime < 1.5f)
        {
            downTime += Time.fixedDeltaTime;
            yield return null;
        }

        DragStart(eventData);
        downTime = 0f;
    }

    private void DragStart(PointerEventData eventData)
    {
        //그 위치에서 활성화 
        dragImage.transform.position = transform.position;
        if (TryGetComponent(out copyItem))
        {
            dragImage.gameObject.SetActive(true);
            dragImage.sprite = copyItem.Sprite;           
        }
    }
}
