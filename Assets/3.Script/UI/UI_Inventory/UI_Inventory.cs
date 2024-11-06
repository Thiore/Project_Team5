using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_Inventory : MonoBehaviour
{
    [SerializeField] private GameObject[] inventoryslots;
    [SerializeField] private GameObject itemInformation;

    private List<Item> items;

    // UI_Press >> 드래그하는거 해둠 

    // 1. 이넘 타입에 따라 퀵슬롯 갈지 인벤 갈지 
    // 2. 추후 SaveManager에 소지 아이템 던질 준비 하기 
    // 3. 퀵슬롯도 상속 시키기 

    // 1. 다른 오브젝트 클릭 시 화면에 띄우게 하기 
    // 2. 조합 
    // 3. 3D렌더링 

    private void Awake()
    {
        items = new List<Item>();
    }

    public void GetItemTouch(Item item)
    {
        for (int i = 0; i < inventoryslots.Length; i++)
        {
            if (!inventoryslots[i].TryGetComponent(out Item notusethis))
            {
                Debug.Log("인벤 " + inventoryslots[i].name);
                inventoryslots[i].SetActive(true);
                Item itemUI = inventoryslots[i].AddComponent<Item>();
                itemUI.PutInInvenItem(item);

                if(itemUI.TryGetComponent(out Image sprite))
                {
                    sprite.sprite = itemUI.sprite;
                }

                Destroy(item.gameObject);
            }
            break;
        }

        items.Add(item);
    }



    public void CurrentHaveItem()
    {
        foreach(Item item in items)
        {
            item.ID 
        }
    }

}
