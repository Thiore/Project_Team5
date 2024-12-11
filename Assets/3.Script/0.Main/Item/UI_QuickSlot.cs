using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_QuickSlot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private int id;
    public int slotID { get => id; }
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
            
            image.sprite = null;
            image.enabled = false;
            UI_InvenManager.Instance.SortInvenSlot(id);
            this.item = null;
            gameObject.SetActive(false);
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
            if(!UI_InvenManager.Instance.dragImage.gameObject.activeInHierarchy)
                UI_InvenManager.Instance.dragImage.gameObject.SetActive(true);
        }
        
    }

    public void OnDrag(PointerEventData eventData)
    {
        if(isDragging)
        {
            // 이건 끝나는 곳 확인 할 필요가 있음 
            UI_InvenManager.Instance.dragImage.transform.position = eventData.position;
            switch (id)
            {
                case 5:
                    DragRayToSlide(id, eventData, false);
                    break;
                case 9:
                    DragRayToSlide(id, eventData, false);
                    break;
                case 10:
                    DragRayToSlide(id, eventData, false);
                    break;
                default:
                    break;
            }
        }
        
       
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (UI_InvenManager.Instance.dragImage.gameObject.activeInHierarchy)
        {
            UI_InvenManager.Instance.dragImage.gameObject.SetActive(false);
        }
        isDragging = false;
        Ray ray = Camera.main.ScreenPointToRay(eventData.position);
        if (Physics.Raycast(ray, out RaycastHit hit, TouchManager.Instance.getTouchDistance, TouchManager.Instance.getTouchableLayer))
        {
            if (hit.collider.TryGetComponent(out ToggleOBJ toggle))
            {
                if (id.Equals(toggle.getObjectIndex))
                {
                    toggle.InteractionObject();
                    SetinvenByID(id, true);
                }
            }
            if (hit.collider.TryGetComponent(out TouchPuzzleCanvas puzzle))
            {
                switch (id)
                {
                    case 5:
                        if(DragRayToSlide(id, eventData, true))
                        {
                            puzzle.InteractionObject(item.id);
                            SetinvenByID(id, true);
                        }
                        break;
                    case 9:
                        if (DragRayToSlide(id, eventData, true))
                        {
                            puzzle.InteractionObject(item.id);
                            SetinvenByID(id, true);
                        }
                        break;
                    case 10:
                        if (DragRayToSlide(id, eventData, true))
                        {
                            puzzle.InteractionObject(item.id);
                            SetinvenByID(id, true);
                        }
                        break;
                        //if (Physics.Raycast(ray, out RaycastHit slideHit, TouchManager.Instance.getTouchDistance, LayerMask.NameToLayer("SlideObject")))
                        //{
                        //    if (slideHit.collider.TryGetComponent(out HideSlide slide))
                        //    {
                        //        if (slide.IsInteracted(id, true))
                        //        {
                        //            for (int i = 0; i < puzzle.getInteractionIndex.Count; i++)
                        //            {
                        //                if (item.id.Equals(puzzle.getInteractionIndex[i]))
                        //                {
                        //                    puzzle.InteractionObject(item.id);
                        //                    SetinvenByID(id, true);
                        //                    break;
                        //                }
                        //            }
                        //        }
                        //    }
                        //}
                        //break;
                    default:
                        for (int i = 0; i < puzzle.getInteractionIndex.Count; i++)
                        {
                            if (item.id.Equals(puzzle.getInteractionIndex[i]))
                            {
                                puzzle.InteractionObject(item.id);
                                SetinvenByID(id, true);
                                break;
                            }
                        }
                        break;

                }

            }
        }
        
    }

    private bool DragRayToSlide(int objId, PointerEventData eventData, bool touchEnd)
    {
        Ray ray = Camera.main.ScreenPointToRay(eventData.position);
        LayerMask slideLayerMask = LayerMask.GetMask("SlideObject");
        if (Physics.Raycast(ray, out RaycastHit hit, 5f, slideLayerMask))
        {
            
            if (hit.collider.TryGetComponent(out HideSlide slide))
            {
                if (slide.isClear) return false;

                if (tempSlide != null)
                {
                    if (touchEnd)
                    {
                        if(tempSlide.IsInteracted(id, touchEnd))
                        {
                            tempSlide = null;
                            return true;
                        }
                        else
                        {
                            tempSlide = null;
                            return false;
                        }

                    }
                    else
                    {
                        if (!slide.Equals(tempSlide))
                        {
                            tempSlide.HideMaterial();
                            tempSlide = slide;
                            tempSlide.IsInteracted(id, touchEnd);
                        }
                    }
                    return false;
                }
                else
                {
                    if (slide.getObjIndex.Equals(id))
                    {
                        tempSlide = slide;
                        tempSlide.IsInteracted(id, touchEnd);
                    }
                }
            }
           
        }
        return false;
    }
}
