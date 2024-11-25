using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_QuickSlot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    //private int id = -1;
    //public int SlotID { get => id; }
    public Item item { get; private set; }
    [SerializeField] private Image image;
    private bool isDragging = false;

    public void SetinvenByID(int id, bool isInteraction = false)
    {
        if(!isInteraction)
        {
            //this.id = id;
            this.item = DataManager.instance.GetItemInfoById(id);
            image.sprite = item.sprite;
            image.enabled = true;
        }
        else
        {
            UI_InvenManager.Instance.SortInvenSlot(id);
            //this.id = -1;
            this.item = null;
            image.sprite = null;
            image.enabled = false;
        }
       
    }


    public void OnBeginDrag(PointerEventData eventData)
    {
        if(!isDragging)
        {
            Debug.Log("비긴시작");
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
            // 이건 끝나는 곳 확인 할 필요가 있음 
            UI_InvenManager.Instance.dragImage.transform.position = eventData.position;
        }
       
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Ray ray = Camera.main.ScreenPointToRay(eventData.position);
        if (Physics.Raycast(ray, out RaycastHit hit, TouchManager.Instance.getTouchDistance, TouchManager.Instance.getTouchableLayer))
        {
            if (hit.collider.TryGetComponent(out TouchPuzzleCanvas toggle))
            {
                for (int i = 0; i < toggle.getInteractionIndex.Length; i++)
                {
                    if (item.id.Equals(toggle.getInteractionIndex[i]))
                    {
                        toggle.isInteracted = true;
                        SaveManager.Instance.UpdateObjectState(toggle.getFloorIndex, toggle.getInteractionIndex[i], true);

                        if (UI_InvenManager.Instance.dragImage.gameObject.activeSelf)
                        {
                            UI_InvenManager.Instance.dragImage.gameObject.SetActive(false);
                        }
                        SetinvenByID(toggle.getInteractionIndex[i],true);
                        break;
                    }
                }
            }
            if (hit.collider.TryGetComponent(out PlayOBJ puzzle))
            {
                for (int i = 0; i < puzzle.getObjectIndex.Length; i++)
                {
                    if (item.id.Equals(puzzle.getObjectIndex[i]))
                    {
                        puzzle.InteractionCount();
                        SaveManager.Instance.UpdateObjectState(puzzle.getFloorIndex, puzzle.getObjectIndex[i], true);
                        if (UI_InvenManager.Instance.dragImage.gameObject.activeSelf)
                        {
                            UI_InvenManager.Instance.dragImage.gameObject.SetActive(false);
                        }
                        SetinvenByID(puzzle.getObjectIndex[i], true);
                        break;
                    }
                }
            }
        }
    }
}
