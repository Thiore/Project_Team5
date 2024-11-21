using UnityEngine;
using UnityEngine.EventSystems;

public class Item
{
    public int id;
    public string name;
    public eItemType eItemType;
    public int elementindex;
    public int combineindex;
    public string tableName;
    public bool isused;
    public int usecount;
    public bool isfix;
    public Sprite sprite;


    public Item(int id, string name, int eItemType, int elementindex,
                    int combineindex, string tableName, bool isused, int usecount, bool isfix, string spritename)
    {
        this.id = id;
        this.name = name;
        this.eItemType = (eItemType)eItemType;
        this.elementindex = elementindex;
        this.combineindex = combineindex;
        this.tableName = tableName;
        this.isused = isused;
        this.usecount = usecount;
        this.isfix = isfix;
        this.sprite = Resources.Load<Sprite>($"UI/Item/{spritename}");
        Debug.Log(this.sprite);
    }

}
