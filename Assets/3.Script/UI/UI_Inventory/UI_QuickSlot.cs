using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_QuickSlot : MonoBehaviour/*, IBeginDragHandler, IDragHandler*/
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

    private IEnumerator HoldDragStart(PointerEventData eventData)
    {
        while (downTime < 1.5f)
        {
            downTime += Time.fixedDeltaTime;
            yield return null;
        }

        DragStart(eventData);
        downTime = 0f;
        Debug.Log("코루틴이 끝난 뒤");
    }

    private void DragStart(PointerEventData eventData)
    {
        //그 위치에서 활성화 
        dragImage.transform.position = transform.position;

        dragImage.gameObject.SetActive(true);
        dragImage.sprite = copyItem.Sprite;

    }

    public void OnBeginDrag(PointerEventData eventData)
    {
       // dragcoroutine = StartCoroutine(HoldDragStart(eventData));
        Debug.Log("퀵 포인트 다운");
    }
}
