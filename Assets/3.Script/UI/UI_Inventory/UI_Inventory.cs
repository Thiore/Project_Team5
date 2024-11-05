using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Inventory : MonoBehaviour
{
    [SerializeField] private GameObject[] inventoryslots;

    private List<Item> items;

    private void Awake()
    {
        items = new List<Item>();




    }



    //ITem 말고 정보만 담고 있는 Ui 전용 스크립트 따로 

    public void GetItemTouch(Item item)
    {
        Debug.Log("실행");
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
            }
            break;
        }

        items.Add(item);


    }


}
