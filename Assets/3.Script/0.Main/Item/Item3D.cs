using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item3D : MonoBehaviour, ITouchable
{
    [SerializeField] private int id;
    public int ID { get => id; }

    [SerializeField] private Transform clue;
    public Item item { get; private set; }
    //[SerializeField] private UI_LerpImage lerpimage;

    private bool isGet;

    private void Awake()
    {
        isGet = false;
    }


    private void OnEnable()
    {
        if(DataSaveManager.Instance.GetItemState(id))
        {
            transform.SetParent(clue);
            transform.localPosition = Vector3.zero;
            transform.rotation = Quaternion.identity;
            transform.localScale *= 10f;
            ClueItem.childItem.Add(id, this);
            isGet = true;
        }
        else
        {
            item = DataSaveManager.Instance.itemData[id];
        }
    }

    public void OnTouchEnd(Vector2 position)
    {
        if(!isGet)
        {
            Ray ray = Camera.main.ScreenPointToRay(position);
            if (Physics.Raycast(ray, out RaycastHit hit, TouchManager.Instance.getTouchDistance, TouchManager.Instance.getTouchableLayer))
            {
                if (hit.collider.gameObject.Equals(gameObject) && gameObject.CompareTag("Item3D"))
                {

                    transform.SetParent(clue);
                    transform.localPosition = Vector3.zero;
                    transform.rotation = Quaternion.identity;
                    transform.localScale *= 10f;
                    ClueItem.childItem.Add(id, this);
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
