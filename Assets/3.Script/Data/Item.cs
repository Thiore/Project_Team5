using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Item : MonoBehaviour, ITouchable, IPointerDownHandler, IPointerUpHandler, IDragHandler
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

    private bool isUI;
    private bool isUsed; 

    private Sprite sprite;
    public Sprite Sprite { get => sprite; private set => sprite = Sprite; }



    private Vector2 firstPos;

    private void Awake()
    {
        isUI = true;

        inven = FindObjectOfType<UI_Inventory>();
        Debug.Log(inven);
        iteminfo = FindObjectOfType<UI_ItemInformation>();
        Debug.Log(iteminfo);
    }


    //private void Update()
    //{
        //if (isUI)
        //{
        //    inven.GetItemTouch(this);
            
        //    Debug.Log("아이템 update");
        //}
    //}

    public void InputItemInfomationByID(int id, ItemData data)
    {
        this.id = id;
        type = (eItemType)data.eItemType;
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
        iteminfo.SetInfoByID(this);
    }


    public void OnTouchStarted(Vector2 position)
    {
        //firstPos = position;//터치 시작위치 저장
        Debug.Log("d");
        if (gameObject.CompareTag("Item3D"))
        {
            inven.GetItemTouch(this);
        }
    }

    public void OnTouchHold(Vector2 position)
    {
        
    }

    public void OnTouchEnd(Vector2 position)
    {
        ////터치 땔때의 위치가 시작위치에서 일정거리 떨어져있을때 아무일도 안일어나도록 하기위해 사용
        //float touchUpDelta = Vector2.Distance(firstPos, position);
        //Debug.Log(touchUpDelta);
        ////거리의 기준을 잡지못해 50으로 임시로 지정했습니다 추후 수정이 필요합니다.
        //if (touchUpDelta<50f&&gameObject.CompareTag("Item3D"))
        //{
        //    inven.GetItemTouch(this);
        //}
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        firstPos = eventData.position;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        //터치 땔때의 위치가 시작위치에서 일정거리 떨어져있을때 아무일도 안일어나도록 하기위해 사용
        float touchUpDelta = Vector2.Distance(firstPos, eventData.position);
        //거리의 기준을 잡지못해 50으로 임시로 지정했습니다 추후 수정이 필요합니다.
        if (touchUpDelta < 50f && gameObject.CompareTag("Item2D"))
        {
            SetInventoryInfomation();
        }
    }

    public void OnDrag(PointerEventData eventData)
    {

    }


}
