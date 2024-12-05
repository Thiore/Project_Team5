using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClueItem : MonoBehaviour
{
    public static ClueItem Instance { get; private set; } = null;

    public Dictionary<int, Item3D> childItem { get; private set; }
    public Item3D activeItem { get; private set; } = null;
    [SerializeField] private GameObject pinObj;
    [SerializeField] private GameObject clueUI;
    public int keyClueId { get; private set; }

    private void Awake()
    {
        Instance = this;
        childItem = new Dictionary<int, Item3D>();
    }
    public void GetItem(int id, Item3D item)
    {
        childItem.Add(id, item);
    }
    public void UseItem(int id)
    {
        if(keyClueId.Equals(id))
        {
            clueUI.SetActive(false);
        }
        childItem.Remove(id);
    }
    public void SetPin(int id)
    {
        activeItem = childItem[id];
        activeItem.gameObject.SetActive(true);
        if(activeItem.item.eItemType.Equals(eItemType.Clue))
        {
            pinObj.SetActive(true);
        }
        else
        {
            pinObj.SetActive(false);
        }
    }
    public void SetClueId()
    {
        keyClueId = activeItem.ID;
    }

}
