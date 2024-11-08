public enum eItemType
{
    element = 0,
    clue,
    trigger,
    quick
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
