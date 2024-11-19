using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InvenSlot
{
    public GameObject item;
    public TeoItemData data;
    public InvenSlot(GameObject obj, TeoItemData itemData)
    {
        item = obj;
        data = itemData;
    }
}
public class Inventory : MonoBehaviour, IEndDragHandler, IDragHandler, IBeginDragHandler, IPointerUpHandler
{
    
    public static Inventory Instance = null;

    [SerializeField] private GameObject[] invenSlot;
    [SerializeField] private Button optionBtn;
    [SerializeField] private Image info;
    private int infoIndex;
    [SerializeField] private Image dragImage;
    public List<InvenSlot> invenList;
    private GraphicRaycaster raycaster;
    private InvenSlot selectObj;

    private bool isDrag;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        invenList = new List<InvenSlot>();
        isDrag = false;
        selectObj = null;


    }
    public void GetItem(TeoItemData data)
    {
        data.GetItem();
        for(int i = 0; i < invenSlot.Length;i++)
        {
            if (invenSlot[i].activeInHierarchy)
            {
                continue;
            }
            else
            {
                invenList.Add(new InvenSlot(invenSlot[i], data));
                SaveManager.Instance.TeoItemDataSave(data);
                invenList[i].item.SetActive(true);
                
                infoIndex = invenList.Count -1;
                UpdateInformation(data, true);
                break;
            }
        }
    }
    public void UseItem(TeoItemData data)
    {
        data.SetUseCount();

    }

    

    public void OnBeginDrag(PointerEventData eventData)
    {
        if(eventData.selectedObject.CompareTag("ItemSlot"))
        {
            selectObj = invenList.Find(x => x.item.Equals(eventData.selectedObject));
            if(selectObj.data.type.Equals(eItemType.Element))
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

    public void OnDrag(PointerEventData eventData)
    {
        if (!isDrag) return;

        dragImage.transform.position = eventData.position;
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        if (!isDrag) return;

        PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
        List<RaycastResult> results = new List<RaycastResult>();
        raycaster.Raycast(pointerEventData, results);
        if (results.Count > 0)
        {
            foreach (var rayresult in results)
            {
                if (rayresult.gameObject.Equals(info.gameObject))
                {
                   
                    

                }
            }
        }
        dragImage.gameObject.SetActive(false);

    }

    public void OnPointerUp(PointerEventData eventData)
    {
        selectObj = invenList.Find(x => x.item.Equals(eventData.selectedObject));

        infoIndex = invenList.IndexOf(selectObj);
        UpdateInformation(selectObj.data, true);

    }

    private void UpdateInformation(TeoItemData data, bool activeInfo)
    {
        info.sprite = data.sprite;
        DialogueManager.Instance.SetItemNameText("Table_ItemName", data.id);
        DialogueManager.Instance.SetItemExplanationText("Table_ItemExplanation", data.id);
        info.gameObject.SetActive(activeInfo);
    }

    private void CombineItem()
    {
        int combineIndex = selectObj.data.elementIndex;
        InvenSlot infoitem = invenList.Find(x => x.data.sprite == info.sprite);
        infoitem.item.SetActive(false);
        invenList.Remove(infoitem);
        selectObj.item.SetActive(false);
        invenList.Remove(selectObj);
        selectObj = null;
        //TeoItemManager.Instance.item_Dic.
    }
}
