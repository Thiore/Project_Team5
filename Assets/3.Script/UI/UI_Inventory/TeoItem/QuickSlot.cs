using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class QuickSlot : MonoBehaviour, IEndDragHandler, IDragHandler, IBeginDragHandler, IPointerUpHandler
{
    public static QuickSlot Instance = null;

    public GameObject[] quickSlot;

    public List<InvenSlot> quickList;

    [SerializeField] private Image dragImage;

    private bool isDrag;

    private InvenSlot selectObj;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        quickList = new List<InvenSlot>();
        isDrag = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!isDrag) return;

        dragImage.transform.position = eventData.position;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (EventSystem.current.gameObject.CompareTag("QuickSlot"))
        {
            selectObj = quickList.Find(x => x.item.Equals(EventSystem.current.gameObject));
            if (!selectObj.data.id.Equals(2))
            {
                isDrag = true;
                dragImage.sprite = selectObj.data.sprite;
                dragImage.transform.position = eventData.position;
                dragImage.gameObject.SetActive(true);
            }
            else
            {
                selectObj = null;
                return;
            }

        }
        
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!isDrag) return;
        
            Ray ray = Camera.main.ScreenPointToRay(eventData.position);
            if (Physics.Raycast(ray, out RaycastHit hit, TouchManager.Instance.getTouchDistance, TouchManager.Instance.getTouchableLayer))
            {
                if (hit.collider.TryGetComponent(out TouchPuzzleCanvas toggle))
                {
                    for (int i = 0; i < toggle.getInteractionIndex.Length; i++)
                    {
                        if (selectObj.data.id.Equals(toggle.getInteractionIndex[i]))
                        {
                            toggle.isInteracted = true;
                            SaveManager.Instance.UpdateObjectState(toggle.getFloorIndex, toggle.getInteractionIndex[i], true);
                            Debug.Log("여기1?");

                            UseItem(toggle.getInteractionIndex[i]);
                        }
                    }
                }
                if (hit.collider.TryGetComponent(out PlayOBJ puzzle))
                {
                    for (int i = 0; i < puzzle.getObjectIndex.Length; i++)
                    {
                        if (selectObj.data.id.Equals(puzzle.getObjectIndex[i]))
                        {
                            puzzle.InteractionCount();
                            SaveManager.Instance.UpdateObjectState(puzzle.getFloorIndex, puzzle.getObjectIndex[i], true);
                            Debug.Log("여기2?");

                        UseItem(puzzle.getObjectIndex[i]);
                        }
                    }

                }
            }

        
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        if (EventSystem.current.gameObject.CompareTag("QuickSlot"))
        {
            selectObj = quickList.Find(x => x.item.Equals(EventSystem.current.gameObject));
            if (selectObj.data.id.Equals(2))
            {
                PlayerManager.Instance.flashLight.enabled = !PlayerManager.Instance.flashLight.enabled;
            }
            else
            {
                selectObj = null;
                return;
            }

        }
    }

    public void AddQuickItem(TeoItemData data)
    {
        for (int j = 0; j < quickSlot.Length; j++)
        {
            if (quickSlot[j].activeInHierarchy)
            {
                continue;
            }
            else
            {
                quickList.Add(new InvenSlot(quickSlot[j], data));
                quickList[j].item.transform.GetChild(0).TryGetComponent(out Image quickSprite);
                quickSprite.sprite = data.sprite;
                break;
            }

        }
    }
    
    private void UseItem(int id)
    {
        if(selectObj.data.id.Equals(id))
        {
            selectObj.item.transform.GetChild(0).TryGetComponent(out Image selectImage);
            selectImage.sprite = null;
            selectObj.item.gameObject.SetActive(false);
            Inventory.Instance.UseItem(selectObj.data);
            if (selectObj.data.useCount.Equals(0))
            {
                selectObj.item.SetActive(false);

                quickList.Remove(selectObj);

            }
            selectObj = null;
        }
        
        
    }
}
