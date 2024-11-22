using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Newtonsoft.Json;
using System.Linq;

public class DataManager : MonoBehaviour
{
    public static DataManager instance = null;

    private Dictionary<int, Item> dicItem;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            InitDic();
            LoadAllData();
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

    private void InitDic()
    {
        dicItem = new Dictionary<int, Item>();
    }

    private void LoadAllData()
    {
        LoadItemData(); 

    }

    private void LoadItemData()
    {
        string itemJson = Resources.Load<TextAsset>("Data/Json/Item_Data").text;
        dicItem = JsonConvert.DeserializeObject<Item[]>(itemJson).ToDictionary(x => x.id, x => x);
        Debug.Log(dicItem.Count);
    }


    private void LoadSceanData(Scene scene, LoadSceneMode mode)
    {
        if (!scene.name.Equals("Lobby"))
        {
            Dictionary<int, ItemSaveData> savedata = SaveManager.Instance.itemsavedata;
            if (savedata.Count > 0)
            {
                Item3D[] items = FindObjectsOfType<Item3D>();

                foreach (KeyValuePair<int, ItemSaveData> data in savedata)
                {
                    for (int i = 0; i < items.Length; i++)
                    {
                        if (items[i].ID.Equals(data.Key))
                        {
                            if (data.Key.Equals(2))
                            {
                                UI_InvenManager.Instance.FlashLightOn();
                            }
                            else
                            {
                                items[i].gameObject.SetActive(false);
                            }

                            if (data.Value.isused.Equals(false))
                            {
                                Debug.Log(data.Key);
                                Item item = GetItemInfoById(data.Key);
                                Debug.Log(item.name);
                                item.isused = data.Value.isused;
                                UI_InvenManager.Instance.GetItemByID(item);
                            }
                        }
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
