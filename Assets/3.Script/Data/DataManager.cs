using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Newtonsoft.Json;
using System.Linq;
using System.IO;

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

        SceneManager.sceneLoaded += LoadSceanData; // ���� �ε� �ɶ����� 

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
