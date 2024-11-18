using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_ItemInformation : MonoBehaviour/*, IDropHandler*/
{
    [SerializeField] private UI_Inventory inven;
    private int id;
    public int ID { get; private set; }
    private int elementindex;
    public int Elementindex { get => elementindex; }

    private void Start()
    {
        inven = PlayerManager.Instance.ui_inventory;
    }

    //private void OnEnable()
    //{
    //    if(inven.MyItems.Count>0)
    //}

    public void SetInfoByID(Item item)
    {
        this.id = item.ID;
        this.elementindex = item.Elementindex;
        DialogueManager.Instance.SetItemNameText("Table_ItemName", id);
        DialogueManager.Instance.SetItemExplanationText("Table_ItemExplanation", id);
        if(TryGetComponent(out Image image))
        {
            image.sprite = item.Sprite;
        }
    }
    

    public void Combine(PointerEventData eventData)
    {
        if (eventData.pointerDrag.gameObject.TryGetComponent(out Item item))
        {
            if (!item.ID.Equals(id) && item.Elementindex.Equals(elementindex))
            {
                Debug.Log("조합가능");

                Item combineitem = DataManager.instance.GetItemCombineIndex(elementindex);
                inven.GetCombineItem(combineitem);

                inven.DestroyElement(item.Elementindex);

                // 애들 비워줘야 됨 

            }
        }
    }


    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("afsdafsdafsdafsd!!!#!!!!!!!!");
        if (eventData.pointerDrag.gameObject.TryGetComponent(out Item item))
        {
            if (!item.ID.Equals(id) && item.Elementindex.Equals(elementindex))
            {
                Debug.Log("조합가능");

                Item combineitem = DataManager.instance.GetItemCombineIndex(elementindex);
                inven.GetCombineItem(combineitem);

                inven.DestroyElement(item.Elementindex);

                // 애들 비워줘야 됨 

            }
        }

    }

}
