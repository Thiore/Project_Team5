using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeoItemManager : MonoBehaviour
{
    public static TeoItemManager Instance { get; private set; } = null;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }    
    }
    public Dictionary<int, ItemData> item_Dic { get; private set; }
    public Dictionary<int, ItemSaveData> itemSave_Dic { get; private set; }

    private void OnEnable()
    {
        item_Dic = DataManager.instance.dicItemData;
        itemSave_Dic = SaveManager.Instance.itemsavedata;
    }
}
