using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_Inventory : MonoBehaviour
{
    [SerializeField] private GameObject[] inventoryslots;
    [SerializeField] private GameObject itemInformation;

    [SerializeField] private TMP_Text itemName;
    [SerializeField] private TMP_Text itemInfomation;

    private List<Item> items;

    private void Awake()
    {
        items = new List<Item>();
    }

    public void GetItemTouch(Item item)
    {
        Debug.Log("실행");
        for (int i = 0; i < inventoryslots.Length; i++)
        {
            if (!inventoryslots[i].TryGetComponent(out Item notusethis))
            {
                Debug.Log("인벤 " + inventoryslots[i].name);
                inventoryslots[i].SetActive(true);
                Item itemUI = inventoryslots[i].AddComponent<Item>();
                itemUI.PutInInvenItem(item);

                if(itemUI.TryGetComponent(out Image sprite))
                {
                    sprite.sprite = itemUI.sprite;
                }

                Destroy(item.gameObject);
                DialogueManager.Instance.SetDialogue("Table_ItemName", itemUI.ID);
                itemName = DialogueManager.Instance.dialogueText;
                Debug.Log(itemName);
                DialogueManager.Instance.SetDialogue("Table_ItemExplanation", itemUI.ID);
                itemInfomation = DialogueManager.Instance.dialogueText;
                Debug.Log(itemInfomation);


            }
            break;
        }

        items.Add(item);


    }


}
