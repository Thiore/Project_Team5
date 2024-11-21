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

        SceneManager.sceneLoaded += LoadSceanData; 
    }


    private void InitDic()
    {
        dicItem = new Dictionary<int, Item>();
    }

    private void LoadAllData()
    {
        LoadItemData(); //���� ������ ������ �ε�

    }

    // ������ ������ȭ �� �ٷ� Item Ÿ������ ��ȯ 
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
            if(savedata.Count > 0)
            {
                Item3D[] items = FindObjectsOfType<Item3D>();

                foreach (KeyValuePair<int, ItemSaveData> data in savedata)
                {
                    for (int i = 0; i < items.Length; i++)
                    {
                        if (items[i].ID.Equals(data.Key))
                        {
                            items[i].gameObject.SetActive(false);

                            if (data.Value.isused.Equals(false))
                            {
                                UI_InvenManager.Instance.GetItemByID(data.Key);
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
