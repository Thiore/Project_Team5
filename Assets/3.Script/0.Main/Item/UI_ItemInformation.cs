using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_ItemInformation : MonoBehaviour, IDropHandler
{
    private int id = -1;
    public int ID { get; private set; }

    private Image image;


    public void SetInfoByItem(Item item)
    {
        if (id.Equals(-1))
        {
            gameObject.SetActive(true);

        }
        if (!item.id.Equals(2))
        {
            switch (id)
            {
                case -1:
                    break;
                case 2:
                    ClueItem.Instance.childItem[0].gameObject.SetActive(false);
                    break;
                default:
                    ClueItem.Instance.childItem[id].gameObject.SetActive(false);
                    break;
            }

            this.id = item.id;
                ClueItem.Instance.SetPin(id);
        }
        else
        {
            switch (id)
            {
                case -1:
                    break;
                case 2:
                    ClueItem.Instance.childItem[0].gameObject.SetActive(false);
                    break;
                default:
                    ClueItem.Instance.childItem[id].gameObject.SetActive(false);
                    break;
            }

            this.id = item.id;
            switch (id)
            {
                case 2:
                    ClueItem.Instance.SetPin(0);
                    break;
            }


        }



        DialogueManager.Instance.SetItemNameText("Table_ItemName", id);
        DialogueManager.Instance.SetItemExplanationText("Table_ItemExplanation", id);

        if(image == null)
            TryGetComponent(out image);

        image.sprite = item.sprite;

    }
    

    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log(eventData.pointerDrag.gameObject.name);
        Debug.Log(eventData.pointerEnter.gameObject.name);
        if (eventData.pointerDrag.gameObject.TryGetComponent(out UI_InventorySlot item))
        {
            UI_InvenManager.Instance.dragImage.gameObject.SetActive(false);
            UI_InvenManager.Instance.Combine(item, id);

        }
    }

}
