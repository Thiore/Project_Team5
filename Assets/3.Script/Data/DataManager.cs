using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.Linq;
using static UnityEditor.Progress;

public class DataManager : MonoBehaviour
{
    public static DataManager instance = null;

    private Dictionary<int, ItemData> dicItemData;
    private Dictionary<int, Item> dicItem;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        InitDic();
        LoadAllData();
    }


    private void InitDic()
    {
        dicItemData = new Dictionary<int, ItemData>();
        dicItem = new Dictionary<int, Item>();
    }

    private void LoadAllData()
    {
        LoadItemData();

        ShowMyInfoTest();
    }



    private void LoadItemData()
    {
        string itemJson = Resources.Load<TextAsset>("Data/Json/Item_Data").text;
        dicItemData = JsonConvert.DeserializeObject<ItemData[]>(itemJson).ToDictionary(x => x.id, x => x);

        foreach (KeyValuePair<int, ItemData> itemdata in dicItemData)
        {
            Debug.Log(itemdata.Value.name);
        }
    }




    public Item GetItemById(int id)
    {
        return dicItem[id];
    }


    public void ShowMyInfoTest()
    {
        foreach (KeyValuePair<int, Item> item in dicItem)
        {
            Debug.Log(item.Value.name);
        }
    }

}
