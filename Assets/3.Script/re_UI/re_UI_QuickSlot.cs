using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class re_UI_QuickSlot : MonoBehaviour
{
    [SerializeField] private int id = -1;
    public int SlotID { get => id; }
    [SerializeField] private re_Item item;
    [SerializeField] private Image image;


    public void SetinvenByID(re_Item item)
    {
        this.id = item.id;
        this.item = item;
        this.image.sprite = item.sprite;
    }




}
