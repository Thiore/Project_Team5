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
    [SerializeField] private TeoItem[] CombineObjects;

    [SerializeField] private GameObject[] invenSlot;
    
    [SerializeField] private Button optionBtn;
    [SerializeField] private Image dragImage;
    [SerializeField] private Image info;


    public List<InvenSlot> invenList;

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
            if (invenSlot[i].activeSelf)
            {
                continue;
            }
            else
            {
                invenList.Add(new InvenSlot(invenSlot[i], data));
                SaveManager.Instance.TeoItemDataSave(data);
                invenList[i].item.transform.GetChild(0).TryGetComponent(out Image sprite);
                sprite.sprite = data.sprite;
                invenList[i].item.SetActive(true);
                UpdateInformation(data, true);
                if(data.type.Equals(eItemType.Quick)|| data.type.Equals(eItemType.Trigger))
                {
                    QuickSlot.Instance.AddQuickItem(data);
                    
                }
                break;
            }
        }
    }
    public void UseItem(TeoItemData data)
    {
        data.SetUseCount();
        if(data.useCount.Equals(0))
        {
            InvenSlot item = invenList.Find(x => x.data == data);
            item.item.SetActive(false);
            item.data.SetUseCount();
            invenList.Remove(item);
            if(invenList.Count>0)
            {
                UpdateInformation(invenList[invenList.Count - 1].data, true);
            }
            else
            {
                UpdateInformation(null, true);
            }
            
        }

    }

    

    public void OnBeginDrag(PointerEventData eventData)
    {
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
        List<RaycastResult> results = new List<RaycastResult>();
        GraphicRaycaster raycaster = null;
        raycaster.Raycast(pointerEventData, results);

        if (results.Count > 0)
        {
            foreach (var rayresult in results)
            {
                if (rayresult.gameObject.CompareTag("ItemSlot"))
                {
                    Debug.Log(EventSystem.current.gameObject);
                    selectObj = invenList.Find(x => x.item.Equals(EventSystem.current.gameObject));
                    if (selectObj.data.type.Equals(eItemType.Element))
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
        GraphicRaycaster raycaster = null;
        raycaster.Raycast(pointerEventData, results);
        if (results.Count > 0)
        {
            foreach (var rayresult in results)
            {
                if (rayresult.gameObject.Equals(info.gameObject))
                {
                    CombineItem();
                }
            }
        }
        dragImage.gameObject.SetActive(false);

    }

    public void OnPointerUp(PointerEventData eventData)
    {
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
        List<RaycastResult> results = new List<RaycastResult>();
        GraphicRaycaster raycaster = null;
        raycaster.Raycast(pointerEventData, results);

        if (results.Count > 0)
        {
            foreach (var rayresult in results)
            {
                if (rayresult.gameObject.CompareTag("ItemSlot"))
                {
                    selectObj = invenList.Find(x => x.item.Equals(EventSystem.current.gameObject));

                    UpdateInformation(selectObj.data, true);
                }
            }
        }
    }

    private void UpdateInformation(TeoItemData data, bool activeInfo)
    {
        if(data != null)
        {
            info.sprite = data.sprite;
            DialogueManager.Instance.SetItemNameText("Table_ItemName", data.id);
            DialogueManager.Instance.SetItemExplanationText("Table_ItemExplanation", data.id);
        }
        
        info.gameObject.SetActive(activeInfo);
    }

    private void CombineItem()
    {
        int combineIndex = selectObj.data.elementIndex;
        InvenSlot infoitem = invenList.Find(x => x.data.sprite == info.sprite);
        infoitem.item.SetActive(false);
        infoitem.data.SetUseCount();
        invenList.Remove(infoitem);
        selectObj.item.SetActive(false);
        selectObj.data.SetUseCount();
        invenList.Remove(selectObj);
        selectObj = null;
        foreach(TeoItem item in CombineObjects)
        {
            if(item.itemData.combineIndex.Equals(combineIndex))
            {
                GetItem(item.itemData);
            }
        }
    }
}
