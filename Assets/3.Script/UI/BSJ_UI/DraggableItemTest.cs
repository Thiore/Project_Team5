using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class DraggableItemTest : MonoBehaviour
{
    private Image sourceImage;
    public GameObject combineImage;
    private Vector3 originalPosition;
    private PlayerInput playerInput;
    private bool isHolding = false;

    private void Awake()
    {
        sourceImage = GetComponent<Image>();
        playerInput = GetComponent<PlayerInput>();

        //Hold Action 구독
        playerInput.actions["Hold"].started += OnHoldStarted;
        playerInput.actions["Hold"].canceled += OnHoldCanceled;
    }

    private void OnHoldStarted(InputAction.CallbackContext context)
    {
        StartCoroutine(LongPress_co());
    }

    private void OnHoldCanceled(InputAction.CallbackContext context)
    {
        StopAllCoroutines();
        combineImage.SetActive(false);
    }

    private IEnumerator LongPress_co()
    {
        yield return new WaitForSeconds(1f);
        isHolding = true;

        //CombineImage 활성화 및 SourceImage설정
        if (combineImage != null)
        {
            combineImage.SetActive(true);
            Image combineImageComponet = combineImage.GetComponent<Image>();
            combineImageComponet.sprite = sourceImage.sprite; //터치된 UI의 Sprite로 설정

            Color color = combineImageComponet.color;
            color.a = 0.7f; // 약간 투명하게
            combineImageComponet.color = color;

            combineImage.transform.position = originalPosition; //터치 위치로 이동
        }

        while (isHolding)
        {
            combineImage.transform.position = Mouse.current.position.ReadValue();
            yield return null;
        }
    }

    private void CheckForDrop(PointerEventData eventData)
    {
        //Raycast로 UI체크
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
                Image targetImage = result.gameObject.GetComponent<Image>();
                if (targetImage != null)
                {
                    //Sprite 이름으로 확인
                    if (targetImage.sprite.name == "2")
                    {
                        sourceImage.sprite = Resources.Load<Sprite>("3");
                        targetImage.sprite = null;
                        targetImage.gameObject.SetActive(false);
                    }
                }
            }
        }

    }

    
    


}
