using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_QuickSlot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private int id;
    //public int SlotID { get => id; }
    public Item item { get; private set; }
    [SerializeField] private Image image;
    private bool isDragging = false;

    private HideSlide tempSlide = null;
    

    public void SetinvenByID(int id, bool isInteraction = false)
    {
        if(!isInteraction)
        {
            this.id = id;
            this.item = DataSaveManager.Instance.itemData[id];
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
            //switch (id)
            //{
            //    case 5://큰 슬라이드퍼즐
            //        DragRayToSlide(this.id, eventData, false);
            //        break;
            //    case 6:
            //        DragRayToSlide(this.id, eventData, false);
            //        break;
            //    case 7:
            //        DragRayToSlide(this.id, eventData, false);
            //        break;
            //    default:
            //        break;
            //}
        }
        
       
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Ray ray = Camera.main.ScreenPointToRay(eventData.position);
        if (Physics.Raycast(ray, out RaycastHit hit, TouchManager.Instance.getTouchDistance, TouchManager.Instance.getTouchableLayer))
        {
            if (hit.collider.TryGetComponent(out TouchPuzzleCanvas toggle))
            {
                for (int i = 0; i < toggle.getInteractionIndex.Count; i++)
                {
                    if (item.id.Equals(toggle.getInteractionIndex[i]))
                    {
                        DataSaveManager.Instance.UpdateGameState(toggle.getFloorIndex, toggle.getInteractionIndex[i], true);
                       
                        if (UI_InvenManager.Instance.dragImage.gameObject.activeSelf)
                        {
                            UI_InvenManager.Instance.dragImage.gameObject.SetActive(false);
                        }
                        toggle.InteractionObject(item.id);
                        SetinvenByID(id,true);
                        break;
                    }
                }
            }
            if (hit.collider.TryGetComponent(out PlayOBJ puzzle))
            {
                for (int i = 0; i < puzzle.getObjectIndex.Count; i++)
                {
                    if (item.id.Equals(puzzle.getObjectIndex[i]))
                    {
                        DataSaveManager.Instance.UpdateGameState(puzzle.getFloorIndex, puzzle.getObjectIndex[i], true);
                        
                        if (UI_InvenManager.Instance.dragImage.gameObject.activeSelf)
                        {
                            UI_InvenManager.Instance.dragImage.gameObject.SetActive(false);
                        }
                        puzzle.InteractionObject(item.id);
                        SetinvenByID(id, true);
                        break;
                    }
                }
            }

            //if(hit.collider.TryGetComponent(out HideSlide slide))
            //{
            //    if(slide.)
            //}
        }
    }

    //private void DragRayToSlide(int objId, PointerEventData eventData, bool touchEnd)
    //{
    //    Ray ray = Camera.main.ScreenPointToRay(eventData.position);
    //    if (Physics.Raycast(ray, out RaycastHit hit, TouchManager.Instance.getTouchDistance, TouchManager.Instance.getTouchableLayer))
    //    {
    //        if (hit.collider.TryGetComponent(out HideSlide slide))
    //        {
    //            if(tempSlide != null)
    //            {
    //                tempSlide.HideMaterial();
    //                tempSlide = slide;
    //            }
    //            if()
    //            if (slide.IsInteracted(id, touchEnd))
    //            {

    //            }
    //        }
    //        else
    //        {
    //            tempSlide.HideMaterial();
    //            tempSlide = null;
    //        }
    //    }
    //}
}
