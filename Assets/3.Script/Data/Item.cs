using System.Collections;
using System.Collections.Generic;
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


    [SerializeField] private UI_Inventory inven;

    private bool isUI;
    private bool isUsed; 

    private Sprite sprite;
    public Sprite Sprite { get => sprite; private set => sprite = Sprite; }

    private ReadInputData inputdata = null;


    private void Awake()
    {
        if (TryGetComponent(out inputdata))
        {
            isUI = true;
        }

        inven = FindObjectOfType<UI_Inventory>();
    }


    private void Update()
    {
        if (isUI && inputdata.isTouch)
        {
            inven.GetItemTouch(this);
            inputdata.TouchTap();
            Debug.Log("아이템 update");
        }
    }

    public void InputItemInfomationByID(int id, ItemData data)
    {
        this.id = id;
        type = (eItemType)data.eItemType;
        Debug.Log(type);
        elementindex = data.elementindex;
        combineindex = data.combineindex;
        tableName = data.tableName;
        isfix = data.isfix;
        spritename = data.spritename;
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
        DialogueManager.Instance.SetItemNameText("Table_ItemName", id);
        DialogueManager.Instance.SetItemExplanationText("Table_ItemExplanation", id);
    }


    public void OnTouchStarted(Vector2 position)
    {
        if (gameObject.CompareTag("Item3D"))
        {
            inven.GetItemTouch(this);
        }
    }

    public void OnTouchHold(Vector2 position)
    {
        throw new System.NotImplementedException();
    }

    public void OnTouchEnd(Vector2 position)
    {
        throw new System.NotImplementedException();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (gameObject.CompareTag("Item2D"))
        {
            Debug.Log("ㅕㅑ 서택");
            SetInventoryInfomation();
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }
}
