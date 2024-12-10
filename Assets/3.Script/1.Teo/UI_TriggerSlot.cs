using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_TriggerSlot : MonoBehaviour, IUITouchable
{
    public Item item { get; private set; } = null;
    private Image image;
    private TriggerList triggerList;
    private TriggerButton triggerButton;

    

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
        if (triggerButton.image == null)
        {
            triggerButton.SetTriggerByItem(item);
        }
        this.item = item;
        image.sprite = item.sprite;
    }

    public void OnUIStarted(PointerEventData data)
    {
       
    }

    public void OnUIHold(PointerEventData data)
    {
        
    }

    public void OnUIEnd(PointerEventData data)
    {
        triggerButton.SetTriggerByItem(item);
        triggerList.CloseList();
    }
}
