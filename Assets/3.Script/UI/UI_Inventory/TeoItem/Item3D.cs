using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item3D : TeoItem, ITouchable
{
    private TeoLerp lerpImage;
    private Inventory inven;

    protected override void OnEnable()
    {
        base.OnEnable();
        lerpImage = PlayerManager.Instance.getLerpImage;
        inven = PlayerManager.Instance.getInven;
    }
    public void OnTouchEnd(Vector2 position)
    {
        Ray ray = Camera.main.ScreenPointToRay(position);
        if (Physics.Raycast(ray, out RaycastHit hit, TouchManager.Instance.getTouchDistance, TouchManager.Instance.getTouchableLayer))
        {
            if (hit.collider.gameObject.Equals(gameObject)&&lerpImage.isLerp.Equals(false))
            {
                lerpImage.gameObject.SetActive(true);
                lerpImage.InputMovementInventory(itemData, position);
                inven.GetItem(itemData);
            }
        }
    }

    public void OnTouchHold(Vector2 position)
    {
        
    }

    public void OnTouchStarted(Vector2 position)
    {
        
    }

    
}
