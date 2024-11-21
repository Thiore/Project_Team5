using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class UI_InvenManager : MonoBehaviour
{
    public static UI_InvenManager Instance { get; private set; } = null;

    [SerializeField] public List<UI_InventorySlot> invenslots;
    [SerializeField] public List<UI_QuickSlot> quickslots;

    [SerializeField] private GameObject inven;
    [SerializeField] public UI_ItemInformation iteminfo;
    [SerializeField] public Image dragimage;

    // 플래시 라이트 조합 되면 트리거로 빠지면서 이거 true 
    private bool isFlashlight;
    [SerializeField] private Light flashright;
    public void FlashLightOn()
    {
        flashright.gameObject.SetActive(true);
        flashright.enabled = true;
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void GetItemByID(Item item)
    {
        iteminfo.SetInfoByItem(item);
        AddSlotItem(item);
        // 세이브 생각 해야함  >> Item3D에 해둠

        // 타입에 따라 추후 추가 작업
        switch (item.eItemType)
        {
            case eItemType.Element:
                break;
            case eItemType.Clue:
                break;
            case eItemType.Trigger:
                AddQuickItem(item);
                break;
            case eItemType.Quick:
                AddQuickItem(item);
                break;

        }


    }


    public void AddSlotItem(Item item)
    {
        for (int i = 0; i < invenslots.Count; i++)
        {
            if (invenslots[i].SlotID.Equals(-1))
            {
                invenslots[i].SetinvenByID(item);
                break;
            }
        }
    }

    public void AddQuickItem(Item quick)
    {
        for (int i = 0; i < quickslots.Count; i++)
        {
            if (quickslots[i].SlotID.Equals(-1))
            {
                quickslots[i].SetinvenByID(quick);
                break;
            }
        }
    }

    public void OpenInventory()
    {
        for(int i = 0; i < invenslots.Count; i++)
        {
            if (!invenslots[i].SlotID.Equals(-1))
            {
                invenslots[i].gameObject.SetActive(true);
            }
            else
            {
                invenslots[i].gameObject.SetActive(false);
            }
        }
    }

    public void Combine(UI_InventorySlot itemslot, int id)
    {
        int firstelement = DataManager.instance.GetItemElementIndex(itemslot.SlotID);
        int secondelement = DataManager.instance.GetItemElementIndex(id);

        if (firstelement.Equals(secondelement) && !itemslot.SlotID.Equals(id)) 
        {
            switch (firstelement)
            {
                case 10: // 손전등
                    Item item = DataManager.instance.GetItemInfoById(2);
                    GetItemByID(item);
                    SaveManager.Instance.InputItemSavedata(item);
                    flashright.enabled = true;
                    break;
                                   
            }

            SortInvenSlot(itemslot.SlotID);
            SortInvenSlot(id);
            OpenInventory();
        }
    }


    public void SortInvenSlot(int id)
    {
        for(int i = 0; i < invenslots.Count; i++)
        {
            if (invenslots[i].SlotID.Equals(id))
            {
                SaveManager.Instance.InputItemSavedata(DataManager.instance.GetItemInfoById(id));
                invenslots[i].SetInvenEmpty();
                invenslots[i].transform.SetParent(null);
                invenslots[i].gameObject.SetActive(false);
                invenslots[i].transform.SetParent(inven.transform);
                break;
            }
        }
    }


}
