using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class re_UI_InvenManager : MonoBehaviour, ITouchable
{
    [SerializeField] List<re_UI_InvenSlot> invenslots;
    [SerializeField] List<re_UI_QuickSlot> quickslots;

    [SerializeField] UI_LerpImage lerpimage;

    public void OnTouchHold(Vector2 position)
    {

    }

    public void OnTouchStarted(Vector2 position)
    {

    }

    public void OnTouchEnd(Vector2 position)
    {
        Ray ray = Camera.main.ScreenPointToRay(position);
        if (Physics.Raycast(ray, out RaycastHit hit, TouchManager.Instance.getTouchDistance, TouchManager.Instance.getTouchableLayer))
        {
            if (TryGetComponent() hit.collider.gameObject.Equals(gameObject) && gameObject.CompareTag("Item3D"))
            {
              //  lerpimage.gameObject.SetActive(true);
              //  lerpimage.InputMovementInventory(this, position);  이건 나중에
               if(TryGetComponent(out ))
               
                
            }
        }
    }


    public void InputInventoryItem(int id)
    {

    }

}
