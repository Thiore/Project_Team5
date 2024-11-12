using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Newtonsoft.Json;
using System.Linq;

public class DataManager : MonoBehaviour
{
    public static DataManager instance = null;

    private Dictionary<int, ItemData> dicItemData;
    private Dictionary<int, Item> dicItem;
    private List<Item> items;

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
        SceneManager.sceneLoaded += LoadSceanData;
    }


    private void InitDic()
    {
        dicItemData = new Dictionary<int, ItemData>();
        dicItem = new Dictionary<int, Item>();
        items = new List<Item>();
    }

    private void LoadAllData()
    {
        LoadItemData();

    }



    private void LoadItemData()
    {
        string itemJson = Resources.Load<TextAsset>("Data/Json/Item_Data").text;
        dicItemData = JsonConvert.DeserializeObject<ItemData[]>(itemJson).ToDictionary(x => x.id, x => x);

    }


    private void LoadSceanData(Scene scene, LoadSceneMode mode)
    {
        items = FindObjectsOfType<Item>().ToList();
        foreach (KeyValuePair<int, ItemData> itemdata in dicItemData)
        {
            if (items.Count > 0)
            {
                for (int i = 0; i < items.Count; i++)
                {
                    if (itemdata.Value.name == items[i].gameObject.name)
                    {
                        items[i].InputItemInfomationByID(itemdata.Key, itemdata.Value);
                        Sprite sprite = Resources.Load<Sprite>($"UI/Item/{itemdata.Value.spritename}");

                        if (sprite != null)
                        {

                            items[i].SetSprite(sprite);
                        }
                    }
                }
            }
        }
    }


    public ItemData GetItemDataInfoById(int id)
    {
        return dicItemData[id];
    }

    public Item GetItemCombineIndex(int combineindex)
    {
        foreach(Item item in items)
        {
            if (item.Combineindex.Equals(combineindex))
            {
                Debug.Log("여기");
                return item;
            }
        }

                Debug.Log("널 전");
        return null;
    }


}
