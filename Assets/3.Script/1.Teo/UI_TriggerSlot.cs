using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_TriggerSlot : MonoBehaviour, IPointerClickHandler
{
    public Item item { get; private set; } = null;
    private Image image;
    private TriggerList triggerList;
    private TriggerButton triggerButton;

    private void OnEnable()
    {
        if (this.item == null)
        {
            InitSlot();
        }
    }

    private void InitSlot()
    {
        TryGetComponent(out image);
        transform.parent.TryGetComponent(out triggerList);
        triggerButton = triggerList.getTriggerButton;
    }
    public void SetinvenByItem(Item item)
    {
        if (this.item == null)
        {
            InitSlot();
        }
        if (!triggerButton.gameObject.activeInHierarchy)
        {
            triggerButton.gameObject.SetActive(true);
            triggerButton.SetTriggerByItem(item);
        }
        this.item = item;
        image.sprite = item.sprite;
    }
   
    public void OnPointerClick(PointerEventData eventData)
    {
        if (triggerList.openList_co == null)
        {
            triggerButton.SetTriggerByItem(item);
            triggerList.CloseList();
        }
            

    }
}
