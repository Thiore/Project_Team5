using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ResultFuse : Fuse, ITouchable
{
    private FuseSlot slot;

    public void GetSlot(FuseSlot slot)
    {
        this.slot = slot;
    }

    public void OnTouchEnd(Vector2 position)
    {
        Ray ray = Camera.main.ScreenPointToRay(position);
        if (Physics.Raycast(ray, out RaycastHit hit, TouchManager.Instance.getTouchDistance, TouchManager.Instance.getTouchableLayer))
        {
            //Debug.Log(hit.collider.gameObject.name);
            if (hit.collider.gameObject.Equals(gameObject) && gameObject.activeSelf)
            {
                SetFuseColor(3);
                slot.Image.enabled = true;
                gameObject.SetActive(false);
                slot = null;

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



