using UnityEngine;
public enum eItemType
{
    Element = 0,
    Clue,
    Trigger,
    Quick
}

public class Item
{
    public int id;
    public string name;
    public eItemType eItemType;
    public int elementindex;
    public string tableName;
    public bool isused;
    public Sprite sprite;


    public Item(int id, string name, int eItemType, int elementindex,
                    string tableName, bool isused, string spritename)
    {
        this.id = id;
        this.name = name;
        this.eItemType = (eItemType)eItemType;
        this.elementindex = elementindex;
        this.tableName = tableName;
        this.isused = isused;
        this.sprite = Resources.Load<Sprite>($"UI/Item/{spritename}");
    }

}
