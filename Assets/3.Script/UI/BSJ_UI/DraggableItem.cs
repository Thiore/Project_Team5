using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DraggableItem : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private Image sourceImage;
    public GameObject combineImage;
    private Vector3 originalPosition;

    private void Awake()
    {
        sourceImage = GetComponent<Image>();
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        originalPosition = eventData.position; //초기 터치 위치 저장
        StartCoroutine(LongPress_Co(eventData));
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        StopCoroutine(LongPress_Co(eventData));

        if (combineImage != null)
        {
            combineImage.SetActive(false);
        }
    }

    private IEnumerator LongPress_Co(PointerEventData eventData)
    {
        yield return new WaitForSeconds(1f);

        //CombineImage 활성화 및 SourceImage 설정
        if (combineImage != null)
        {
            combineImage.SetActive(true);
            Image combineImageComponet = combineImage.GetComponent<Image>();
            combineImageComponet.sprite = sourceImage.sprite; //터치된 UI의 Sprite로 설정

            Color color = combineImageComponet.color;
            color.a = 0.7f;
            combineImageComponet.color = color;

            combineImage.transform.position = originalPosition; //터치 위치로 이동
        }

        while (true)
        {
            combineImage.transform.position = eventData.position;
            yield return null;
        }
    }
    //public void OnBeginDrag(PointerEventData eventData)
    //{
    //    //originalPosition = rectTransform.anchoredPosition;
    //    quickSlot.alpha = 0.6f; //드래그 중 반투명
    //    quickSlot.blocksRaycasts = false; //다른 UI 클릭 방지
    //}

    //public void OnDrag(PointerEventData eventData)
    //{
    //    rectTransform.anchoredPosition += eventData.delta; //드래그 위치 업데이트
    //}

    //public void OnEndDrag(PointerEventData eventData)
    //{
    //    quickSlot.alpha = 1f; //드래드 종료 시 불투명
    //    quickSlot.blocksRaycasts = true; //다시 클릭 가능
    //}
}
