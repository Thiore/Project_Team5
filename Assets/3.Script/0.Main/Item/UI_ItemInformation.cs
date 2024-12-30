using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_ItemInformation : MonoBehaviour, IDropHandler
{
    public int id { get; private set; }
    private Image image;


    public void SetInfoByItem(Item item)
    {
        if (!gameObject.activeSelf)
        {
            gameObject.SetActive(true);

        }
        this.id = item.id;
        ClueItem.Instance.SetPin(id);
            
        DialogueManager.Instance.SetItemNameText("Table_ItemName", id);
        DialogueManager.Instance.SetItemExplanationText("Table_ItemExplanation", id);

        if(image == null)
            TryGetComponent(out image);

        image.sprite = item.sprite;

    }
    

    public void OnDrop(PointerEventData eventData)
    {
       // Debug.Log(eventData.pointerDrag.gameObject.name);
        //Debug.Log(eventData.pointerEnter.gameObject.name);
        if (eventData.pointerDrag.gameObject.TryGetComponent(out UI_InventorySlot item))
        {
            UI_InvenManager.Instance.dragImage.gameObject.SetActive(false);
            if(item.item.elementindex>0)
                UI_InvenManager.Instance.Combine(item, id);

        }
    }

}
