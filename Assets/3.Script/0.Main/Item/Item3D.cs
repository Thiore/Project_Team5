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
    [Header("회전이 필요한 아이템의 회전값을 정해주세요")]
    [SerializeField] private float rotX;
    [SerializeField] private float rotY;
    [SerializeField] private float rotZ;

    [Header("조합아이템이 아니라면 0으로 해주세요")]
    [SerializeField] private int combineItem;
    [SerializeField] private GameObject combineObj;

    protected bool isGet;

    private Collider mask;

    

    private void Awake()
    {
        isGet = false;
        item = DataSaveManager.Instance.itemData[id];
        TryGetComponent(out mask);
        
    }
    private void Start()
    {
        if(combineItem>0 || id.Equals(19) || id.Equals(20))
        {
            gameObject.SetActive(false);
            return;
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
        else if(id.Equals(19)||id.Equals(20))
        {
            if (!isLoading)
                DataSaveManager.Instance.UpdateItemState(id);
        }
        else
        {
            
            transform.SetParent(ClueItem.Instance.transform);
            transform.localPosition = Vector3.zero;
            transform.localEulerAngles = new Vector3(rotX, rotY, rotZ);
            transform.localScale *= scaling;
            mask.enabled = false;
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
