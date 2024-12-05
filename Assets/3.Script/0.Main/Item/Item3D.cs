using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item3D : MonoBehaviour, ITouchable
{
    [SerializeField] private int id;
    public int ID { get => id; }

    public Item item { get; private set; }
    //[SerializeField] private UI_LerpImage lerpimage;

    private bool isGet;

    private void Awake()
    {
        isGet = false;
        item = DataSaveManager.Instance.itemData[id];
        if (DataSaveManager.Instance.GetItemState(id))
        {
            isGet = true;
            ClueItem.Instance.GetItem(id, this);
            transform.SetParent(ClueItem.Instance.transform);
            transform.localPosition = Vector3.zero;
        }
    }
   

   

    public void OnTouchEnd(Vector2 position)
    {
        if(!isGet)
        {
            Ray ray = Camera.main.ScreenPointToRay(position);
            if (Physics.Raycast(ray, out RaycastHit hit, TouchManager.Instance.getTouchDistance, TouchManager.Instance.getTouchableLayer))
            {
                if (hit.collider.gameObject.Equals(gameObject))
                {

                    transform.SetParent(ClueItem.Instance.transform);
                    transform.localPosition = Vector3.zero;
                    ClueItem.Instance.GetItem(id,this);
                    isGet = true;
                    DataSaveManager.Instance.UpdateItemState(id);
                    UI_InvenManager.Instance.GetItemByID(item);
                }
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
