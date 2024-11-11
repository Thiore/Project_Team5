using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Inventory : MonoBehaviour
{
    [SerializeField] private GameObject[] inventoryslots;
    [SerializeField] private GameObject[] quickSlots;

    [SerializeField] private GameObject invenBtnPos;
    [SerializeField] private GameObject quickBtnPos;
    [SerializeField] private Image lerpImage;

    private List<Item> myitems;

    // UI_Press >> 드래그하는거 해둠 

    // 1. 이넘 타입에 따라 퀵슬롯 갈지 인벤 갈지 
    // 2. 추후 SaveManager에 소지 아이템 던질 준비 하기 
    // 3. 퀵슬롯도 상속 시키기 

    // 1. 다른 오브젝트 클릭 시 화면에 띄우게 하기 
    // 2. 조합 
    // 3. 3D렌더링 


    // 퀵슬롯 만약 0~3 있는데 2 쓰면 한칸씩 땡겨지게 

    private void Awake()
    {
        myitems = new List<Item>();
    }


    //아이템 Type에 따라 
    public void GetItemTouch(Item item)
    {
        lerpImage.sprite = item.Sprite;
        lerpImage.transform.position = item.transform.position;
        if (!lerpImage.gameObject.activeSelf)
        {
            lerpImage.gameObject.SetActive(true);
        }
        Vector3.Lerp(lerpImage.transform.position, invenBtnPos.transform.position, 10f);


        switch (item.Type)
        {
            case eItemType.Quick:
                AddItemQuick(item);
                break;

            case eItemType.Element:
                AddItemInventory(item);
                break;

            case eItemType.Trigger:
                AddItemQuick(item);
                break;

            case eItemType.Clue:
                AddItemInventory(item);
                break;

            default:
                Debug.Log("아무것도 아닌감?");
                break;

        }

        myitems.Add(item);
        lerpImage.gameObject.SetActive(false);
    }


    public void AddItemInventory(Item item)
    {

        for (int i = 0; i < inventoryslots.Length; i++)
        {
            if (!inventoryslots[i].transform.TryGetComponent(out Item notusethis))
            {
                Item itemUI = inventoryslots[i].AddComponent<Item>();
                itemUI.PutInInvenItem(item);

                if (inventoryslots[i].transform.GetChild(0).TryGetComponent(out Image sprite))
                {
                    sprite.sprite = itemUI.Sprite;
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

    public void GetCombineItem(Item item)
    {
        switch (item.Type)
        {
            case eItemType.Quick:
                AddItemQuick(item);
                break;

            case eItemType.Element:
                AddItemInventory(item);
                break;

            case eItemType.Trigger:
                AddItemQuick(item);
                break;

            case eItemType.Clue:
                AddItemInventory(item);
                break;

            default:
                Debug.Log("아무것도 아닌감?");
                break;

        }

        myitems.Add(item);
    }

    public void OpenInventory()
    {
        for(int i = 0; i < inventoryslots.Length; i++)
        {
            if (inventoryslots[i].TryGetComponent<Item>(out Item item))
            {
                inventoryslots[i].SetActive(true); 
            }
        }
    }

    //아이디 검사해서 있는지 없는지 bool return 

}
