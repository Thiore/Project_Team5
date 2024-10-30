using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[System.Serializable]
public class ItemDatatest
{
    public string itemName;
    public Sprite itemIcon;
    public bool isCombine;
}

public class Slot : MonoBehaviour, IDropHandler
{
    public ItemDatatest currentItem; //슬롯에 담긴 아이템
    public ItemDatatest[] possibleCombines; //조합 가능한 아이템

    public void OnDrop(PointerEventData eventData)
    {
        DraggableItem draggableItem = eventData.pointerDrag.GetComponent<DraggableItem>();

        if (draggableItem != null)
        {
            //현재 슬롯에 담긴 아이템과 조합
            TryCombineItem(draggableItem);
        }
    }
    
    private void TryCombineItem(DraggableItem draggableItem)
    {
        //현재 슬롯에 아이템이 없으면 드래그한 아이템을 추가
        if (currentItem == null)
        {
            //슬롯에 아이템 추가
            currentItem = draggableItem.GetComponent<ItemDatatest>();
            //드래그한 아이템 제거
            Destroy(draggableItem.gameObject);
            return;
        }
        ItemDatatest draggedItem = draggableItem.GetComponent<ItemDatatest>();

        //조합
        if (IsCombine(currentItem, draggedItem))
        {
            ItemDatatest comibnedItem = CombineItems(currentItem, draggedItem);

            if (comibnedItem != null)
            {
                //조합 결과로 슬롯의 아이이템 업데이트
                currentItem = comibnedItem;


                //UI 업데이트


                //드래그한 아이템 제거
                Destroy(draggableItem.gameObject);
               
            }
        }
    }
    private bool IsCombine(ItemDatatest item1, ItemDatatest item2)
    {
        return (item1.itemName == "1" && item2.itemName == "2") ||
                (item1.itemName == "2" && item2.itemName == "1");
    }

    private ItemDatatest CombineItems(ItemDatatest item1, ItemDatatest item2)
    {
        //조합 결과 아이템 생성
        return new ItemDatatest { itemName = "3", itemIcon = null };
    }

    //private void UpdaSlotUI()
    //{ 
    
    //}
}
