using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    
    [SerializeField] private int id;
    [SerializeField] private eItemType type;
    [SerializeField] private int elementindex;
    [SerializeField] private int combineindex;
    [SerializeField] private string tableName;
    [SerializeField] private bool isfix;
    [SerializeField] private string spritename;




    
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


}
