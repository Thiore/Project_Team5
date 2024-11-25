using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Newtonsoft.Json;
using System.Linq;

public class DataManager : MonoBehaviour
{
    public static DataManager instance = null;

    public Dictionary<int, Item> dicItem { get; private set; }
    public Dictionary<int, ItemSaveData> savedata { get; private set; }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            LoadItemData();
        }
        else
        {
            Destroy(gameObject);

        }

    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += LoadSceanData;
    }



    private void LoadItemData()
    {
        string itemJson = Resources.Load<TextAsset>("Data/Json/Item_Data").text;
        dicItem = JsonConvert.DeserializeObject<Item[]>(itemJson).ToDictionary(x => x.id, x => x);
        Debug.Log(dicItem.Count);
    }


    private void LoadSceanData(Scene scene, LoadSceneMode mode)
    {
        savedata = SaveManager.Instance.itemsavedata;
        if (!scene.name.Equals("Lobby"))
        {
            
            if (savedata.Count > 0)
            {
                //Item3D[] items = FindObjectsOfType<Item3D>();

                foreach (KeyValuePair<int, ItemSaveData> data in savedata)
                {
                    if (data.Value.isused.Equals(false))
                    {
                        Item item = GetItemInfoById(data.Key);
                        item.isused = data.Value.isused;
                        UI_InvenManager.Instance.GetItemByID(item, true);
                    }
                }
            }
        }
    }

    public Item GetItemInfoById(int id)
    {
        return dicItem[id];
    }

    public int GetItemElementIndex(int id)
    {
        return dicItem[id].elementindex;
    }

}
