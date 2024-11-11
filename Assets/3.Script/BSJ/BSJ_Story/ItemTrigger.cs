using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemTrigger : MonoBehaviour, ITouchable
{
    // 자신의 인덱스 Item
    [SerializeField] private int itemExplanationIndex;
    [SerializeField] private int itemNum;

    //인벤토리
    private GameObject mainPlayer;
    private GameObject canvas;
    private GameObject inventory;
    private UI_Inventory uiInventory;

    //상호작용 독백 출력
    private void ItemText()
    {
        if (gameObject.activeSelf)
        {
            DialogueManager.Instance.SetDialogue("Table_ItemExplanation", itemExplanationIndex);
        }
    }


    //Inventory 찾기
    private void FindObjectUI()
    {
        mainPlayer = GameObject.FindGameObjectWithTag("Player");

        //Canvas찾기
        Transform Canvas_ = mainPlayer.transform.GetChild(5);
        canvas = Canvas_.gameObject;

        //Inventory 찾기
        if (canvas != null)
        {
            Transform Inventory = canvas.transform.GetChild(1);
            inventory = Inventory.gameObject;

            inventory.TryGetComponent(out uiInventory);
        }

    }

    //아이템 넣기
    private void GetItem()
    {
        if (TryGetComponent(out Item item))
        {
            uiInventory.GetItemTouch(item);
        }
    }

    public void OnTouchStarted(Vector2 position)
    {
        
        ItemText();
        GetItem();
        //uiInventory.GetItemTouch();
    }

    public void OnTouchHold(Vector2 position)
    {
        
    }

    public void OnTouchEnd(Vector2 position)
    {
     
    }

    private void OnEnable()
    {
        FindObjectUI();
    }
}
