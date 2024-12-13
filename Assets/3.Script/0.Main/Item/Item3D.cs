using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item3D : MonoBehaviour, ITouchable
{
    [SerializeField] private int id;
    public int ID { get => id; }

    public Item item { get; protected set; }

    [Header("확대시 기본 사이즈를 정해주세요")]
    [Range(1f,50f)]
    [SerializeField] private float scaling;
    [Header("확대시의 Pivot을 정해주세요")]
    [Range(-2f, 2f)]
    [SerializeField] private float pivotX;
    [Range(-2f, 2f)]
    [SerializeField] private float pivotY;
    [Range(-2f, 2f)]
    [SerializeField] private float pivotZ;
    private Vector3 scalingPivot;

    [Header("조합아이템이 아니라면 0으로 해주세요")]
    [SerializeField] private int combineItem;
    [SerializeField] private GameObject combineObj;

    protected bool isGet;

    

    private void Awake()
    {
        isGet = false;
        item = DataSaveManager.Instance.itemData[id];
        scalingPivot = new Vector3(pivotX, pivotY, pivotZ);
        
    }
    private void Start()
    {
        if(combineItem>0)
        {
            gameObject.SetActive(false);
        }
    }




    public void OnTouchEnd(Vector2 position)
    {
        
        if(combineItem.Equals(0)&&!isGet)
        {
            Ray ray = Camera.main.ScreenPointToRay(position);
            if (Physics.Raycast(ray, out RaycastHit hit, TouchManager.Instance.getTouchDistance, TouchManager.Instance.getTouchableLayer))
            {
                if (hit.collider.gameObject.Equals(gameObject))
                {
                    GetItem(false);
                }
            }
        }
    }

    public void GetItem(bool isLoading = true)
    {
        if (combineItem > 0)
        {
            switch (combineItem)
            {
                case 1:
                    combineObj.TryGetComponent(out FlashLight light);
                    light.SetUseFlashLight();

                    break;
            }

        }
        else
        {
            transform.SetParent(ClueItem.Instance.transform);
            transform.localPosition = Vector3.zero;
            transform.Translate(scalingPivot);
            transform.localScale *= scaling;
            if (!isLoading)
                DataSaveManager.Instance.UpdateItemState(id);
        }
        

        isGet = true;
        

        UI_InvenManager.Instance.GetItemByID(item, isLoading,this);
    }
    public void UseItem()
    {
        isGet = true;
        gameObject.SetActive(false);
    }
        

    public void OnTouchHold(Vector2 position)
    {

    }

    public void OnTouchStarted(Vector2 position)
    {

    }
    
}
