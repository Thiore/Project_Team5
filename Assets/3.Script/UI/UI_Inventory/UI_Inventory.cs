using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_Inventory : MonoBehaviour
{
    [SerializeField] private GameObject[] inventoryslots;
    [SerializeField] private GameObject[] quickSlots;

    private List<Item> myitems;

    // UI_Press >> 드래그하는거 해둠 

    // 1. 이넘 타입에 따라 퀵슬롯 갈지 인벤 갈지 
    // 2. 추후 SaveManager에 소지 아이템 던질 준비 하기 
    // 3. 퀵슬롯도 상속 시키기 

    // 1. 다른 오브젝트 클릭 시 화면에 띄우게 하기 
    // 2. 조합 
    // 3. 3D렌더링 

    private void Awake()
    {
        myitems = new List<Item>();
    }

    public void GetItemTouch(Item item)
    {
        Debug.Log("겟터치발동");
        switch (item.Type)
        {
            case eItemType.quick:
                Debug.Log("퀵발동");
                AddItemQuick(item);
                break;

            case eItemType.element:
                Debug.Log("인벤발동");
                AddItemInventory(item);
                break;

            case eItemType.trigger:
                Debug.Log("xmflrj발동");
                AddItemQuick(item);
                break;

            case eItemType.clue:
                Debug.Log("clue 발동");
                AddItemInventory(item);
                break;

        }
        myitems.Add(item);
    }


    public void AddItemInventory(Item item)
    {
        for (int i = 0; i < inventoryslots.Length; i++)
        {
            if (!inventoryslots[i].transform.TryGetComponent(out Item notusethis))
            {
                Debug.Log("인벤 " + inventoryslots[i].name);
                inventoryslots[i].SetActive(true);
                Item itemUI = inventoryslots[i].AddComponent<Item>();
                itemUI.PutInInvenItem(item);

                if (inventoryslots[i].transform.GetChild(0).TryGetComponent(out Image sprite))
                {
                    sprite.sprite = itemUI.Sprite;
                    Debug.Log(sprite.sprite.name);
                }

                Destroy(item.gameObject);
                break;
            }
        }
    }

    public void AddItemQuick(Item item)
    {
        for (int i = 0; i < quickSlots.Length; i++)
        {
            if (!quickSlots[i].TryGetComponent(out Item notusethis))
            {
                Debug.Log("퀵 " + quickSlots[i].name);
                quickSlots[i].SetActive(true);
                quickSlots[i].transform.GetChild(0).gameObject.SetActive(true);
                Item itemUI = quickSlots[i].AddComponent<Item>();
                itemUI.PutInInvenItem(item);

                if (itemUI.transform.GetChild(0).TryGetComponent(out Image sprite))
                {
                    sprite.sprite = itemUI.Sprite;
                }

                Destroy(item.gameObject);
                break;
            }
        }
    }

    public void CurrentHaveItem()
    {
        foreach (Item item in myitems)
        {

        }
    }

}
