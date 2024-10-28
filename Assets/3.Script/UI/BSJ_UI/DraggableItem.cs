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
            CheckForDrop(eventData);
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

    private void CheckForDrop(PointerEventData eventData)
    {
        // Raycast로 충돌한 UI 체크
        PointerEventData pointerData = new PointerEventData(EventSystem.current)
        {
            position = eventData.position
        };

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);

        foreach (var result in results)
        {
            //레이어 검사
            if (result.gameObject.layer == LayerMask.NameToLayer("QuickSlot"))
            {
                Image tartgetImage = result.gameObject.GetComponent<Image>();
                if (tartgetImage != null)
                {
                    //Sprite 이름으로 확인
                    if (tartgetImage.sprite.name == "2")
                    {
                        sourceImage.sprite = Resources.Load<Sprite>("3");
                        
                        
                        tartgetImage.sprite = null;
                        tartgetImage.gameObject.SetActive(false);
                    }
                }
            }
        }
    }
}
