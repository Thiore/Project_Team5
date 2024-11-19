using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class re_UI_InvenManager : MonoBehaviour
{
    public static re_UI_InvenManager Instance { get; private set; } = null;

    [SerializeField] public List<re_UI_InvenSlot> invenslots;
    [SerializeField] public List<re_UI_QuickSlot> quickslots;
    private UI_ItemInformation iteminfo;

    // 플래시 라이트 조합 되면 트리거로 빠지면서 이거 true 
    private bool isFlashlight;
    [SerializeField] private Light flashright;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        
    }

    public void GetItemByID(int id)
    {
        re_Item item = re_DataManager.instance.GetItemInfoById(id);
        AddSlotItem(item);
       

        // 타입에 따라 추후 추가 작업
        switch (item.eItemType)
        {
            case eItemType.Element:
                break;
            case eItemType.Clue:
                break;
            case eItemType.Trigger:
                break;
            case eItemType.Quick:
                AddQuickItem(item);
                break;

        }


    }


    public void AddSlotItem(re_Item item)
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

    public void AddQuickItem(re_Item quick)
    {
        for(int i = 0; i < quickslots.Count; i++)
        {
            if (quickslots[i].SlotID.Equals(-1))
            {
                quickslots[i].SetinvenByID(quick);
                break;
            }
        }
    }
    


}
