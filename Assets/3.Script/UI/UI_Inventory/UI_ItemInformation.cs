using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_ItemInformation : MonoBehaviour, IPointerUpHandler, IDropHandler
{
    [SerializeField] UI_Inventory inven;
    private int id;
    public int ID { get; private set; }
    private int elementindex;
    public int Elementindex { get => elementindex; }

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



    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log(eventData.pointerEnter.gameObject.name);
        Debug.Log("실행되나");

    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag.gameObject.TryGetComponent(out Item item))
        {
            if(!item.ID.Equals(id) && item.Elementindex.Equals(elementindex))
            {
                Debug.Log("조합가능");

                Item combineitem = DataManager.instance.GetItemCombineIndex(elementindex);
                inven.GetCombineItem(combineitem);

                // 애들 비워줘야 됨 
                
            }
        }
    
    }

}
