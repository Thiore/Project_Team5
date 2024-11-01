using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    private int id;
    private eItemType type;
    private int elementindex;
    private int combineindex;
    private string tableName;
    private bool isfix;
    private string spritename;




    
    public void InputItemInfomationByID(int id)
    {
        ItemData data = DataManager.instance.GetItemDataInfoById(id);

        id = data.id;
        type = data.type;
        elementindex = data.elementindex;
        combineindex = data.combineindex;
        tableName = data.tableName;
        isfix = data.isfix;
        spritename = data.spritename;
    }


}
