using UnityEngine;
using UnityEngine.EventSystems;

public class Item : MonoBehaviour, ITouchable, IPointerDownHandler, IPointerUpHandler
{

    [SerializeField] private int id;
    [SerializeField] private eItemType type;
    [SerializeField] private int elementindex;
    [SerializeField] private int combineindex;
    [SerializeField] private string tableName;
    [SerializeField] private bool isfix;
    [SerializeField] private string spritename;

    public int ID { get => id; }
    public eItemType Type { get => type; }
    public int Elementindex { get => elementindex; }
    public int Combineindex { get => combineindex; }


    [SerializeField] private UI_Inventory inven;
    [SerializeField] private UI_ItemInformation iteminfo;
    [SerializeField] private UI_LerpImage lerpimage;

    private int usecount;
    public int Usecount { get => usecount; }

    private bool isget;
    public bool IsGet { get => isget; }


    private Sprite sprite;
    public Sprite Sprite { get => sprite; private set => sprite = Sprite; }
    private Vector2 firstPos;

    private void Start()
    {

        inven = PlayerManager.Instance.ui_inventory;
        iteminfo = PlayerManager.Instance.ui_iteminfo;
        lerpimage = PlayerManager.Instance.ui_lerpImage;
        // �̰� Ȱ��ȭ�� ã���� ���� ����ߵ� 
    }

    public void InputItemInfomationByID(int id, ItemData data)
    {
        this.id = id;
        type = (eItemType)data.eItemType;
        elementindex = data.elementindex;
        combineindex = data.combineindex;
        tableName = data.tableName;
        isfix = data.isfix;
        spritename = data.spritename;

        if (id.Equals(9) || id.Equals(13))
        {
            usecount = 2;
        }
        else
        {
            usecount = 1;
        }

    }

    public void SetSprite(Sprite sprite)
    {
        this.sprite = sprite;
    }

    public void PutInInvenItem(Item item)
    {
        this.id = item.id;
        type = item.type;
        elementindex = item.elementindex;
        combineindex = item.combineindex;
        tableName = item.tableName;
        isfix = item.isfix;
        spritename = item.spritename;
        sprite = item.sprite;
    }

    public void SetInventoryInfomation()
    {
        iteminfo.SetInfoByID(this);
    }


    public void OnTouchStarted(Vector2 position)
    {

    }

    public void OnTouchHold(Vector2 position)
    {

    }

    public void OnTouchEnd(Vector2 position)
    {
        Ray ray = Camera.main.ScreenPointToRay(position);
        if (Physics.Raycast(ray, out RaycastHit hit, TouchManager.Instance.getTouchDistance, TouchManager.Instance.getTouchableLayer))
        {
            if (hit.collider.gameObject.Equals(gameObject) && gameObject.CompareTag("Item3D"))
            {
                lerpimage.gameObject.SetActive(true);
                lerpimage.InputMovementInventory(this, position);
                SwitchGetbool();
                inven.GetItemTouch(this);
            }
        }
    }


    public void OnPointerDown(PointerEventData eventData)
    {
        if (gameObject.CompareTag("Item2D"))
        {
            firstPos = eventData.position;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        //��ġ ������ ��ġ�� ������ġ���� �����Ÿ� ������������ �ƹ��ϵ� ���Ͼ���� �ϱ����� ���
        float touchUpDelta = Vector2.Distance(firstPos, eventData.position);
        //�Ÿ��� ������ �������� 50���� �ӽ÷� �����߽��ϴ� ���� ������ �ʿ��մϴ�.
        if (touchUpDelta < 50f && gameObject.CompareTag("Item2D"))
        {
            SetInventoryInfomation();
            if (!iteminfo.gameObject.activeSelf)
            {
                iteminfo.gameObject.SetActive(true);
            }
        }
    }


    public void SwitchGetbool()
    {
        isget = !isget;
    }

    public void UseAndDiscount()
    {
        usecount--;
    }


    public ItemSaveData SetItemSaveData()
    {
        ItemSaveData data = new ItemSaveData();
        data.id = id;
        data.itemusecount = usecount;
        data.itemgetstate = isget;

        return data;
    }

}
