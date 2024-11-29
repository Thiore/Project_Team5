using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_InvenManager : MonoBehaviour
{
    public static UI_InvenManager Instance { get; private set; } = null;

    [SerializeField] private Transform invenList;

    [SerializeField] private UI_InventorySlot invenSlotPrefabs;
    private List<UI_InventorySlot> invenSlots;
    private Queue<UI_InventorySlot> invenSlots_Queue;

    [SerializeField] private Transform quickSlotList;
    [SerializeField] private UI_QuickSlot quickSlotPrefabs;
    private List<UI_QuickSlot> quickSlots;
    private Queue<UI_QuickSlot> quickSlots_Queue;

    [SerializeField] private Transform triggerSlotList;
    [SerializeField] private UI_TriggerSlot triggerSlotPrefabs;
    [SerializeField] private GameObject btnTrigger;
    [SerializeField] private TriggerButton triggerButton;
    private List<UI_TriggerSlot> triggerSlots;
    


    [SerializeField] public UI_ItemInformation iteminfo;

    [SerializeField] public Image dragImage;

    [SerializeField] private RectTransform lerpImage;
    [SerializeField] private float lerpSpeed;
    [SerializeField] private float DelayTime;
    private Queue<Sprite> getItemQueue;
    private Coroutine getItemImage_co = null;
    private WaitForSeconds waitForDelayTime;

    [SerializeField] private Button optionBtn;
    

    [SerializeField] private FlashLight flashLight;


    private void Awake()
    {
        Instance = this;
        getItemQueue = new Queue<Sprite>();
        waitForDelayTime = new WaitForSeconds(DelayTime);
        InitSlots();

    }
    private void Start()
    {
        OnOpenInventory();
        var loadData = DataSaveManager.Instance.itemStateData;
        foreach(KeyValuePair<int,bool> item in loadData)
        {
            if(!item.Value)
            {
                Item data = DataSaveManager.Instance.itemData[item.Key];
                GetItemByID(data, true);
            }
        }
    }
    private void InitSlots()
    {
        InitInvenSlots();

        InitQuickSlots();
    }
    private void InitInvenSlots()
    {
        invenSlots = new List<UI_InventorySlot>();
        invenSlots_Queue = new Queue<UI_InventorySlot>();
        for (int i = 0; i < 5f; i++)
        {
            UI_InventorySlot newObj = Instantiate(invenSlotPrefabs, invenList);
            newObj.gameObject.SetActive(false);
            invenSlots_Queue.Enqueue(newObj);
        }
    }

    public void OnOpenInventory()
    {
        optionBtn.onClick.AddListener(SettingsManager.Instance.OnSettingPage);
    }
    private void InitQuickSlots()
    {
        quickSlots = new List<UI_QuickSlot>();
        quickSlots_Queue = new Queue<UI_QuickSlot>();
        for (int i = 0; i < 5f; i++)
        {
            UI_QuickSlot newObj = Instantiate(quickSlotPrefabs, quickSlotList);
            newObj.gameObject.SetActive(false);
            quickSlots_Queue.Enqueue(newObj);
        }
    }
    /// <summary>
    /// 아이템을 획득했을 때 인벤토리 및 퀵슬롯, 트리거슬롯에 추가해주는 메서드
    /// </summary>
    /// <param name="item">획득한 아이템을 추가해주세요</param>
    /// <param name="isGetItemImage">처음 로딩할 때와 조합아이템은 true값을 입력하시면 획득 이미지가 나타나지 않습니다.</param>
    public void GetItemByID(Item item, bool isGetItemImage = false)
    {
        if(!isGetItemImage)
        {
            GetItemByImage(item);
        }
        
        iteminfo.SetInfoByItem(item);
        AddInventoryItem(item);
        
        switch (item.eItemType)
        {
            case eItemType.Element:
                break;
            case eItemType.Clue:
                break;
            case eItemType.Trigger:
                AddTriggerItem(item);
                break;
            case eItemType.Quick:
                AddQuickItem(item);
                break;

        }


    }


    private void AddInventoryItem(Item item)
    {
        if(invenSlots_Queue == null || invenSlots == null)
        {
            InitInvenSlots();
        }
        if(invenSlots_Queue.Count>0)
        {
            UI_InventorySlot slot = invenSlots_Queue.Dequeue();
            slot.SetinvenByItem(item);
            invenSlots.Add(slot);
            slot.gameObject.SetActive(true);
        }
        else
        {
            UI_InventorySlot newSlot = Instantiate(invenSlotPrefabs, invenList);
            newSlot.SetinvenByItem(item);
            invenSlots.Add(newSlot);
            newSlot.gameObject.SetActive(true);
        }
    }

    private void AddQuickItem(Item quick)
    {
        if (quickSlots_Queue == null || quickSlots == null)
        {
            InitQuickSlots();
        }
        if (quickSlots_Queue.Count > 0)
        {
            UI_QuickSlot slot = quickSlots_Queue.Dequeue();
            slot.SetinvenByID(quick.id);
            quickSlots.Add(slot);
            slot.gameObject.SetActive(true);
        }
        else
        {
            UI_QuickSlot newSlot = Instantiate(quickSlotPrefabs, quickSlotList);
            newSlot.SetinvenByID(quick.id);
            quickSlots.Add(newSlot);
            newSlot.gameObject.SetActive(true);
        }
    }
    private void AddTriggerItem(Item item)
    {
        if(triggerSlots == null)
        {
            triggerSlots = new List<UI_TriggerSlot>();
            btnTrigger.SetActive(true);
            UI_TriggerSlot initSlot = Instantiate(triggerSlotPrefabs, triggerSlotList);
            initSlot.SetinvenByItem(item);
            triggerSlots.Add(initSlot);
            
            return;
        }
        UI_TriggerSlot newSlot = Instantiate(triggerSlotPrefabs, triggerSlotList);
        newSlot.SetinvenByItem(item);
        triggerSlots.Add(newSlot);

    }

    //public void OpenInventory()
    //{
    //    for (int i = 0; i < invenSlots.Count; i++)
    //    {
    //        if (!invenSlots[i].SlotID.Equals(-1))
    //        {
    //            invenSlots[i].gameObject.SetActive(true);
    //        }
    //        else
    //        {
    //            invenSlots[i].gameObject.SetActive(false);
    //        }
    //    }
    //}

    public void Combine(UI_InventorySlot slot, int id)
    {
        int firstelement = DataSaveManager.Instance.itemData[slot.item.id].elementindex;
        int secondelement = DataSaveManager.Instance.itemData[id].elementindex;

        if (firstelement.Equals(secondelement) && !slot.item.id.Equals(id))
        {
            SortInvenSlot(slot.item.id);
            SortInvenSlot(id);
            switch (firstelement)
            {
                case 10:
                    
                    Item item = DataSaveManager.Instance.itemData[2];
                    GetItemByID(item,true);
                    DataSaveManager.Instance.UpdateItemState(item.id);
                    AddTriggerItem(item);
                    flashLight.SetUseFlashLight();
                    break;

            }

            
            //OpenInventory();
        }
    }


    public void SortInvenSlot(int id)
    {
        UI_InventorySlot slot = invenSlots.Find(x => x.item.id.Equals(id));

        DataSaveManager.Instance.UpdateItemState(id);
        //slot.SetInvenEmpty();
        slot.transform.SetAsLastSibling();
        slot.gameObject.SetActive(false);
        invenSlots.Remove(slot);
        invenSlots_Queue.Enqueue(slot);
    }

    private void GetItemByImage(Item item)
    {
        getItemQueue.Enqueue(item.sprite);
        if(getItemImage_co == null)
        {
            getItemImage_co = StartCoroutine(GetItemByImage_co());
        }

    }

    private IEnumerator GetItemByImage_co()
    {
        lerpImage.transform.GetChild(0).TryGetComponent(out Image setImage);
        
        while(getItemQueue.Count>0)
        {
            setImage.sprite = getItemQueue.Peek();
            float lerpImageY;
            float lerpTime = 0f;
            while (lerpTime/lerpSpeed<1f)
            {
                lerpTime += Time.fixedUnscaledDeltaTime;
                lerpImageY = Mathf.Lerp(170f, 0f, lerpTime / lerpSpeed);
                lerpImage.anchoredPosition = 
                    new Vector2(lerpImage.anchoredPosition.x, lerpImageY);
                yield return null;
            }

            yield return waitForDelayTime;

            while (lerpTime / lerpSpeed > 0f)
            {
                lerpTime -= Time.fixedUnscaledDeltaTime;
                lerpImageY = Mathf.Lerp(170f, 0f, lerpTime / lerpSpeed);
                lerpImage.anchoredPosition =
                    new Vector2(lerpImage.anchoredPosition.x, lerpImageY);
                yield return null;
            }
            getItemQueue.Dequeue();
        }
        getItemImage_co = null;
    }
}