using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Newtonsoft.Json;
using System.Linq;
using System.IO;

public class re_DataManager : MonoBehaviour
{
    public static re_DataManager instance = null;

    private Dictionary<int, ItemData> dicItemData; //원본 아이템 데이터
    private Dictionary<int, re_Item> dicItem;
    private Dictionary<int, Sprite> dicsprite;
    private List<Item> items;

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
        
        SceneManager.sceneLoaded += LoadSceanData; // 씬이 로드 될때마다 
     
    }


    private void InitDic()
    {
        dicItemData = new Dictionary<int, ItemData>();
        dicItem = new Dictionary<int, re_Item>();
        dicsprite = new Dictionary<int, Sprite>();
        items = new List<Item>();
    }

    private void LoadAllData()
    {
        LoadItemData(); //원본 아이템 데이터 로드

    }



    private void LoadItemData()
    {
        string itemJson = Resources.Load<TextAsset>("Data/Json/Item_Data").text;
        dicItemData = JsonConvert.DeserializeObject<ItemData[]>(itemJson).ToDictionary(x => x.id, x => x);
        dicItem = JsonConvert.DeserializeObject<re_Item[]>(itemJson).ToDictionary(x => x.id, x => x);

        foreach(var data in dicItemData.Values)
        {
            Sprite sprite = Resources.Load<Sprite>($"UI/Item/{data.spritename}");
            dicsprite.Add(data.id, sprite);
            Debug.Log(sprite.name);
        }
    }


    private void LoadSceanData(Scene scene, LoadSceneMode mode)
    {
        
    }


    public re_Item GetItemInfoById(int id)
    {
        return dicItem[id];
    }

    public Sprite GetItemSpriteById(int id)
    {
        return dicsprite[id];
    }

    public re_Item GetItemByCombineIndex(int combineindex)
    {
        foreach(re_Item item in dicItem.Values)
        {
            if(item.combineindex == combineindex)
            {
                return item; //이거... 두개 리턴해서 어쩌거 하는 방향은...?
            }
        }

        return null;
    }

    


    public Item GetItemCombineIndex(int combineindex)
    {
        foreach(Item item in items)
        {
            if (item.Combineindex.Equals(combineindex))
            {
                return item;
            }
        }
        return null;
    }


    

}
