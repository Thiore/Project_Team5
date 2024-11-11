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
        Item[] listarr = FindObjectsOfType<Item>();

        foreach (KeyValuePair<int, ItemData> itemdata in dicItemData)
        {
            if (listarr.Length > 0)
            {
                for (int i = 0; i < listarr.Length; i++)
                {
                    if (itemdata.Value.name == listarr[i].gameObject.name)
                    {
                        listarr[i].InputItemInfomationByID(itemdata.Key, itemdata.Value);
                        Sprite sprite = Resources.Load<Sprite>($"UI/Item/{itemdata.Value.spritename}");

                        if (sprite != null)
                        {

                            listarr[i].SetSprite(sprite);
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



}
