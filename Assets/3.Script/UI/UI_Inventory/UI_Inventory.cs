using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UI_Inventory : MonoBehaviour
{
    [SerializeField] private GameObject[] inventoryslots;
    public GameObject[] InvenSolt { get => inventoryslots; }
    [SerializeField] private GameObject[] quickSlots;

    [SerializeField] private GameObject invenBtnPos;
    [SerializeField] private GameObject quickBtnPos;
    [SerializeField] private UI_LerpImage lerpImage;

    private List<Item> myitems;
    public List<Item> MyItems { get => myitems; }

    // UI_Press >> 드래그하는거 해둠 

    // 1. 이넘 타입에 따라 퀵슬롯 갈지 인벤 갈지 
    // 2. 추후 SaveManager에 소지 아이템 던질 준비 하기 
    // 3. 퀵슬롯도 상속 시키기 

    // 1. 다른 오브젝트 클릭 시 화면에 띄우게 하기 
    // 2. 조합 
    // 3. 3D렌더링 


    // 퀵슬롯 만약 0~3 있는데 2 쓰면 한칸씩 땡겨지게 

    // 번들키 2회 사용 해야함 

    private void Awake()
    {
        myitems = new List<Item>();
    }

    private void OnEnable()
    {
        for (int i = 0; i < inventoryslots.Length; i++)
        {

            if (inventoryslots[i].TryGetComponent(out Item item))
            {
                inventoryslots[i].gameObject.SetActive(true);
            }
            else
            {
                inventoryslots[i].gameObject.SetActive(false);
            }

        }
    }


    //아이템 Type에 따라 
    public void GetItemTouch(Item item)
    {
        //if (item.gameObject.CompareTag("Item3D"))
        //{
        //    OutPutItemText(item);
        //}

        item.SetItemSaveData();
        SaveManager.Instance.InputItemSavedata(item);

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


        Destroy(item.gameObject);
    }


    public void AddItemInventory(Item item)
    {

        for (int i = 0; i < inventoryslots.Length; i++)
        {
            if (!inventoryslots[i].transform.TryGetComponent(out Item notusethis))
            {
                Item itemUI = inventoryslots[i].AddComponent<Item>();
                itemUI.PutInInvenItem(item);
                itemUI.SwitchGetbool();

                if (inventoryslots[i].transform.GetChild(0).TryGetComponent(out Image sprite))
                {
                    sprite.sprite = itemUI.Sprite;
                }

                break;
            }
        }

        for (int i = 0; i < inventoryslots.Length; i++)
        {

            if (inventoryslots[i].TryGetComponent(out Item uiitem))
            {
                if (uiitem.enabled.Equals(true))
                {
                    inventoryslots[i].SetActive(true);
                }
                else
                {
                    inventoryslots[i].SetActive(false);
                }
            }
            else
            {
                inventoryslots[i].SetActive(false);
            }

        }
    }

    public void AddItemQuick(Item item)
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
                break;
            }
        }

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
                break;
            }
        }
    }

    public void GetCombineItem(Item item)
    {
        //OutPutItemText(item);

        item.SwitchGetbool();
        item.SetInventoryInfomation();
        SaveManager.Instance.InputItemSavedata(item);

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

        
    }


    //미니게임 / 상호적용 하기전에 아이디 검사해서 있는지 없는지 bool return 
    public bool CheckInteraction(int id)
    {
        for (int i = 0; i < inventoryslots.Length; i++)
        {
            if (inventoryslots[i].TryGetComponent<Item>(out Item item))
            {
                if (item.ID.Equals(id))
                {
                    // 트루 반환하니까 아이템 정리 해야됨 
                    // Destroy(item);

                    return true;
                }
            }
        }

        return false;
    }


    public void DestroyElement(int elementindex)
    {
        for (int i = 0; i < inventoryslots.Length; i++)
        {
            if (inventoryslots[i].TryGetComponent(out Item item))
            {
                if (elementindex.Equals(item.Elementindex) && item.Elementindex > 0)
                {
                    item.UseAndDiscount();
                    item.SwitchGetbool();
                    SaveManager.Instance.InputItemSavedata(item);
                    item.enabled = false;
                    Destroy(item);
                }
            }
        }

        for (int i = 0; i < inventoryslots.Length; i++)
        {

            if (inventoryslots[i].TryGetComponent(out Item item))
            {
                if (item.enabled.Equals(true))
                {
                    inventoryslots[i].SetActive(true);
                }
                else
                {
                    inventoryslots[i].SetActive(false);
                }
            }
            else
            {
                inventoryslots[i].SetActive(false);
            }

        }
    }


    private void OutPutItemText(Item item)
    {
        DialogueManager.Instance.SetDialogue("Table_ItemExplanation", item.ID);
    }

}
