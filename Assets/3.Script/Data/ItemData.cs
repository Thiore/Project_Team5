public enum eItemType
{
    Element = 0,
    Clue,
    Trigger,
    Quick
}

public class ItemData
{
    public int id;
    public string name; 
    public int eItemType;
    public int elementindex;
    public int combineindex;
    public string tableName;
    public bool isfix;
    public string spritename;

}
