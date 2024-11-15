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

    // UI_Press >> �巡���ϴ°� �ص� 

    // 1. �̳� Ÿ�Կ� ���� ������ ���� �κ� ���� 
    // 2. ���� SaveManager�� ���� ������ ���� �غ� �ϱ� 
    // 3. �����Ե� ��� ��Ű�� 

    // 1. �ٸ� ������Ʈ Ŭ�� �� ȭ�鿡 ���� �ϱ� 
    // 2. ���� 
    // 3. 3D������ 


    // ������ ���� 0~3 �ִµ� 2 ���� ��ĭ�� �������� 

    // ����Ű 2ȸ ��� �ؾ��� 

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


    //������ Type�� ���� 
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
                Debug.Log("�ƹ��͵� �ƴѰ�?");
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
                itemUI.SwitchGetbool();

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
                Debug.Log("�ƹ��͵� �ƴѰ�?");
                break;

        }

        
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


    public void LoadItem(Item item)
    {

        item.SetItemSaveData();
        SaveManager.Instance.InputItemSavedata(item);

        switch (item.Type)
        {
            case eItemType.Quick:
                SetItemQuickLoad(item);
                break;

            case eItemType.Element:
                SetItemLoad(item);
                break;

            case eItemType.Trigger:
                SetItemQuickLoad(item);
                break;

            case eItemType.Clue:
                SetItemLoad(item);
                break;

            default:
                Debug.Log("�ƹ��͵� �ƴѰ�?");
                break;

        }
    }

    public void SetItemLoad(Item item)
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

    public void SetItemQuickLoad(Item item)
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

}
