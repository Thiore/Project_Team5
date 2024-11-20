using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_ItemInformation : MonoBehaviour, IDropHandler
{
    [SerializeField] UI_InvenManager invenmanager;
    private int id;
    public int ID { get; private set; }
    private int elementindex;
    public int Elementindex { get => elementindex; }


    public void SetInfoByItem(re_Item item)
    {
        this.id = item.id;
        DialogueManager.Instance.SetItemNameText("Table_ItemName", id);
        DialogueManager.Instance.SetItemExplanationText("Table_ItemExplanation", id);
        if(TryGetComponent(out Image image))
        {
            image.sprite = item.sprite;
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log(eventData.pointerDrag.gameObject.name);
        Debug.Log(eventData.pointerEnter.gameObject.name);
        if (eventData.pointerDrag.gameObject.TryGetComponent(out re_UI_InvenSlot item))
        {
            UI_InvenManager.Instance.dragimage.gameObject.SetActive(false);
            item.FragIsDrag();
            UI_InvenManager.Instance.Combine(item, id);

        }
    }

}
