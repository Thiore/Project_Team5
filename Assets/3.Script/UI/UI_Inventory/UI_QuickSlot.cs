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
            PlayerManager.Instance.flashLight.enabled = !PlayerManager.Instance.flashLight.enabled;
        }
        else
        {
            Ray ray = Camera.main.ScreenPointToRay(eventData.position);
            if (Physics.Raycast(ray, out RaycastHit hit, TouchManager.Instance.getTouchDistance, TouchManager.Instance.getTouchableLayer))
            {
                if (hit.collider.TryGetComponent(out TouchPuzzleCanvas toggle))
                {
                    for (int i = 0; i < toggle.getObjectIndex.Length; i++)
                    {
                        if (copyItem.ID.Equals(toggle.getObjectIndex[i]))
                        {
                            toggle.isInteracted = true;
                            SaveManager.Instance.UpdateObjectState(toggle.getFloorIndex, toggle.getObjectIndex[i], true);
                            Destroy(copyItem);
                        }
                    }
                }
                else if (hit.collider.TryGetComponent(out PlayOBJ puzzle))
                {
                    for (int i = 0; i < puzzle.getObjectIndex.Length; i++)
                    {
                        if (copyItem.ID.Equals(puzzle.getObjectIndex[i]))
                        {
                            puzzle.InteractionCount();
                            SaveManager.Instance.UpdateObjectState(puzzle.getFloorIndex, puzzle.getObjectIndex[i], true);
                            Destroy(copyItem);
                        }
                    }

                }
            }
            if (dragImage.gameObject.activeSelf)
            {
                dragImage.gameObject.SetActive(false);
            }
        }


        Debug.Log("퀵 드래그엔드");
        Debug.Log("여기다가 상호작용");
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (TryGetComponent(out copyItem))
        {
            if (copyItem.ID.Equals(2))
            {
                PlayerManager.Instance.flashLight.enabled = !PlayerManager.Instance.flashLight.enabled;
            }
        }
    }



}
