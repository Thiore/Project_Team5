using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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


        foreach (KeyValuePair<int, ItemData> itemdata in dicItemData)
        {
            Item[] listarr = FindObjectsOfType<Item>();

            if (listarr.Length > 0)
            {
                for (int i = 0; i < listarr.Length; i++)
                {
                    if (itemdata.Value.name == listarr[i].gameObject.name)
                    {
                        listarr[i].InputItemInfomationByID(itemdata.Key, itemdata.Value);
                        Sprite sprite = Resources.Load<Sprite>($"UI/Item/{itemdata.Value.spritename}");

                        Debug.Log(sprite.name);

                        listarr[i].SetSprite(sprite);
                    }
                }
            }


            //if (GameObject.Find(itemdata.Value.name) != null)
            //{
            //    if(GameObject.Find(itemdata.Value.name).TryGetComponent(out Item item))
            //    {
            //        item.InputItemInfomationByID(itemdata.Key, itemdata.Value);
            //    }
            //}
            //else
            //{

            //}
        }
    }




    public ItemData GetItemDataInfoById(int id)
    {
        return dicItemData[id];
    }



}
