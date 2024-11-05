using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Item : MonoBehaviour
{

    [SerializeField] private int id;
    [SerializeField] private eItemType type;
    [SerializeField] private int elementindex;
    [SerializeField] private int combineindex;
    [SerializeField] private string tableName;
    [SerializeField] private bool isfix;
    [SerializeField] private string spritename;

    public int ID { get => id; }


    [SerializeField] private UI_Inventory inven;

    private bool isUI;

    public Sprite sprite { get; private set; }

    private ReadInputData inputdata = null;


    private void Awake()
    {
        if (TryGetComponent(out inputdata))
        {
            isUI = true;
        }

        inven = FindObjectOfType<UI_Inventory>();
        Debug.Log(inven.name);
    }


    private void Update()
    {
        if (isUI && inputdata.isTouch)
        {
            inven.GetItemTouch(this);
            inputdata.TouchTap();
            Debug.Log("æ∆¿Ã≈€ update");
        }
    }

    public void InputItemInfomationByID(int id, ItemData data)
    {
        this.id = id;
        type = data.type;
        elementindex = data.elementindex;
        combineindex = data.combineindex;
        tableName = data.tableName;
        isfix = data.isfix;
        spritename = data.spritename;
    }

    public void SetSprite(Sprite sprite)
    {
        this.sprite = sprite;
    }

    public void PutInInvenItem(Item item)
    {
        this.id = item.id;
        type = item.type;
        elementindex = item.elementindex;
        combineindex = item.combineindex;
        tableName = item.tableName;
        isfix = item.isfix;
        spritename = item.spritename;
        sprite = item.sprite;
    }



}
