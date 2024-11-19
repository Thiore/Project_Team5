using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class re_UI_InvenSlot : MonoBehaviour
{
    [SerializeField] private int id = -1;
    [SerializeField] private re_Item item;
    [SerializeField] private Image image;


    public void SetinvenByID(int id)
    {
        this.id = id;
        item = re_DataManager.instance.GetItemInfoById(id);
        image.sprite = re_DataManager.instance.GetItemSpriteById(id);
    }

}
