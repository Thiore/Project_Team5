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
    private HideBattery tempBattery = null;
    private HideTile tempTile = null;
    private readonly int donInteractionIndex = 1; // '이 아이템이 아닌것같아'
    

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
                    DragRayToSlide(eventData, false);
                    break;
                case 9:
                    DragRayToSlide(eventData, false);
                    break;
                case 10:
                    DragRayToSlide(eventData, false);
                    break;
                case 12:
                    DragRayToObj(eventData, false);
                    break;
                case 17:
                    DragRayToObj(eventData, false);
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
        switch(id)
        {
            case 12:
                if(DragRayToObj(eventData,true))
                {
                    SetinvenByID(id, true);
                    
                }
                break;
            case 17:
                if (DragRayToObj(eventData, true))
                {
                    SetinvenByID(id, true);
                    
                }
                break;
            default:
                Ray ray = Camera.main.ScreenPointToRay(eventData.position);
                if (Physics.Raycast(ray, out RaycastHit hit, TouchManager.Instance.getTouchDistance, TouchManager.Instance.getTouchableLayer))
                {
                    if (hit.collider.TryGetComponent(out NeedItemOBJ needItem))
                    {
                        if (id.Equals(needItem.getObjectIndex))
                        {
                            needItem.InteractionObject();
                            SetinvenByID(id, true);
                            return;
                        }
                        else
                        {
                            DialogueManager.Instance.SetDialogue("Table_StoryB1", donInteractionIndex);
                            return;
                        }
                    }
                    else
                    {
                        if (tempBattery != null)
                        {
                            tempBattery.HideMaterial();
                            return;
                        }


                    }
                    if (hit.collider.TryGetComponent(out TouchPuzzleCanvas puzzle))
                    {
                        switch (id)
                        {
                            case 5:
                                if (DragRayToSlide(eventData, true))
                                {
                                    puzzle.InteractionObject(item.id);
                                    SetinvenByID(id, true);
                                    
                                }
                                else
                                {
                                    DialogueManager.Instance.SetDialogue("Table_StoryB1", donInteractionIndex);
                                    
                                }
                                return;
                            case 9:
                                if (DragRayToSlide(eventData, true))
                                {
                                    puzzle.InteractionObject(item.id);
                                    SetinvenByID(id, true);
                                    
                                }
                                else
                                {
                                    DialogueManager.Instance.SetDialogue("Table_StoryB1", donInteractionIndex);
                                }
                                return;
                            case 10:
                                if (DragRayToSlide(eventData, true))
                                {
                                    puzzle.InteractionObject(item.id);
                                    SetinvenByID(id, true);
                                    
                                }
                                else
                                {
                                    DialogueManager.Instance.SetDialogue("Table_StoryB1", donInteractionIndex);
                                }
                                return;
                            default:
                                for (int i = 0; i < puzzle.getInteractionIndex.Count; i++)
                                {
                                    if (item.id.Equals(puzzle.getInteractionIndex[i]))
                                    {
                                        puzzle.InteractionObject(item.id);
                                        SetinvenByID(id, true);
                                        return;
                                    }
                                }
                                DialogueManager.Instance.SetDialogue("Table_StoryB1", donInteractionIndex);
                                return;

                        }

                    }
                }
                return;
        }
        return;

    }

    private bool DragRayToSlide(PointerEventData eventData, bool touchEnd)
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
            else
            {
                if(tempSlide != null)
                {
                    tempSlide.HideMaterial();
                    tempSlide = null;
                }
            }
        }
        else
        {
            if (tempSlide != null)
            {
                tempSlide.HideMaterial();
                tempSlide = null;
            }
        }
        return false;
    }

    private bool DragRayToObj(PointerEventData eventData, bool touchEnd)
    {
        Ray ray = Camera.main.ScreenPointToRay(eventData.position);
        if (Physics.Raycast(ray, out RaycastHit hit, 3f, TouchManager.Instance.getTouchableLayer))
        {
            switch(id)
            {
                case 12:
                    if (hit.collider.TryGetComponent(out HideTile tile))
                    {
                        if (tempTile != null)
                        {
                            if (touchEnd)
                            {
                                tempTile.IsInteracted(touchEnd);
                                tempTile = null;
                            }
                        }
                        else
                        {
                            tempTile = tile;
                            tempTile.IsInteracted(touchEnd);
                        }
                    }
                    return true;
                case 17:
                    if (hit.collider.TryGetComponent(out HideBattery battery))
                    {
                        if (tempBattery != null)
                        {
                            if (touchEnd)
                            {
                                tempBattery.IsInteracted(touchEnd);
                                tempBattery = null;
                            }
                        }
                        else
                        {
                            tempBattery = battery;
                            tempBattery.IsInteracted(touchEnd);
                        }
                    }
                    return true;
            }
            
        }
        if (tempBattery != null)
        {
            tempBattery.HideMaterial();
            tempBattery = null;
        }
        if(tempTile != null)
        {
            tempTile.HideMaterial();
            tempTile = null;
        }
        return false;
    }
}
