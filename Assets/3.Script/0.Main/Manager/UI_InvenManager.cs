using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_InvenManager : MonoBehaviour
{
    public static UI_InvenManager Instance { get; private set; } = null;

    [SerializeField] public List<UI_InventorySlot> invenslots;
    [SerializeField] public List<UI_QuickSlot> quickslots;

    [SerializeField] private GameObject inven;
    [SerializeField] public UI_ItemInformation iteminfo;
    [SerializeField] public Image dragimage;

    [SerializeField] private RectTransform lerpImage;
    [SerializeField] private float lerpSpeed;
    [SerializeField] private float DelayTime;
    private Queue<Sprite> getItemQueue;
    private Coroutine getItemImage_co = null;
    private WaitForSeconds waitForDelayTime;
    

    // �÷��� ����Ʈ ���� �Ǹ� Ʈ���ŷ� �����鼭 �̰� true 
    private bool isFlashlight;
    [SerializeField] private Light flashright;
    public void FlashLightOn()
    {
        flashright.enabled = true;
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            getItemQueue = new Queue<Sprite>();
            waitForDelayTime = new WaitForSeconds(DelayTime);
        }
    }

    public void GetItemByID(Item item, bool isLoading = false)
    {
        if(!isLoading)
        {
            GetItemByImage(item);
        }
        
        iteminfo.SetInfoByItem(item);
        AddSlotItem(item);
        // ���̺� ���� �ؾ���  >> Item3D�� �ص�

        // Ÿ�Կ� ���� ���� �߰� �۾�
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
        for (int i = 0; i < invenslots.Count; i++)
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
                case 10: // ������
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
        for (int i = 0; i < invenslots.Count; i++)
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