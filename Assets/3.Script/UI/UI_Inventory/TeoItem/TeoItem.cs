using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TeoItemData
{
    public int id { get; private set; }
    public eItemType type { get; private set; }
    public int elementIndex { get; private set; }
    public int combineIndex { get; private set; }
    public string tableName { get; private set; }
    public bool isFix { get; private set; }//임시로 얻은아이템
    public Sprite sprite { get; private set; }
    public int useCount { get; private set; }

    public TeoItemData(int ID, eItemType eType, int eleIndex, int comIndex, string table, Sprite icon, bool isGet = false, int useCount = 0)
    {
        this.id = ID;
        this.type = eType;
        this.elementIndex = eleIndex;
        this.combineIndex = comIndex;
        this.tableName = table;
        this.sprite = icon;
        this.isFix = isGet;
        this.useCount = useCount;
    }

    public void GetItem()
    {
        isFix = true;
    }
    public void SetUseCount()
    {
        useCount -= 1;
    }
}

public class TeoItem : MonoBehaviour
{
    [SerializeField] protected int itemID;

    public TeoItemData itemData { get; private set; }


    protected TeoItemManager itemManager;

    protected virtual void OnEnable()
    {
        SceneManager.sceneLoaded += OnLoadItem;
    }

    protected void OnDisable()
    {
        SceneManager.sceneLoaded -= OnLoadItem;
    }
    private void OnLoadItem(Scene scene, LoadSceneMode mode)
    {
        itemManager = TeoItemManager.Instance;
        if (itemManager.item_Dic.TryGetValue(itemID, out ItemData data))
        {
            if (itemManager.itemSave_Dic.ContainsKey(itemID))
            {
                if (!itemID.Equals(2))
                {
                    if (itemManager.itemSave_Dic[itemID].itemusecount > 0)
                    {
                        Sprite sprite = Resources.Load<Sprite>($"UI/Item/{itemManager.item_Dic[itemID].spritename}");
                        itemData = new TeoItemData(itemID, (eItemType)data.eItemType, data.elementindex,
                              data.combineindex, data.tableName, sprite,
                              itemManager.itemSave_Dic[itemID].itemgetstate, itemManager.itemSave_Dic[itemID].itemusecount);
                    }
                    else
                    {
                        Destroy(gameObject);
                    }
                }
            }
            else
            {
                Sprite sprite = Resources.Load<Sprite>($"UI/Item/{itemManager.item_Dic[itemID].spritename}");
                if (itemID.Equals(9) || itemID.Equals(13))
                {
                    itemData = new TeoItemData(itemID, (eItemType)data.eItemType, data.elementindex,
                           data.combineindex, data.tableName, sprite, false, 2);
                }
                else
                {
                    itemData = new TeoItemData(itemID, (eItemType)data.eItemType, data.elementindex,
                           data.combineindex, data.tableName, sprite, false, 1);
                }

            }
        }
        
            
        
    }
}
