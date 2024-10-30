using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.Linq;

public class DataManager : MonoBehaviour
{
    public static DataManager instance = null;

    private Dictionary<int, ItemData> dicItem; 

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
    }


    private void InitDic()
    {
        dicItem = new Dictionary<int, ItemData>();
    }

    private void LoadAllData()
    {
        
    }



    private void LoadItemData()
    {
        string itemJson = Resources.Load<TextAsset>("Data/Item_Data").text;
        dicItem = JsonConvert.DeserializeObject<ItemData[]>(itemJson).ToDictionary(x => x.id, x => x);

        foreach (KeyValuePair<int, ItemData> item in dicItem)
        {


        }
    }


    public ItemData GetItembyID(int id)
    {
        return dicItem[id];
    }

    


}
