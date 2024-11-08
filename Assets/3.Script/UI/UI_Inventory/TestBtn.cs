using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBtn : MonoBehaviour
{
    [SerializeField] UI_Inventory inven;

    int i = 0;

    public void CreateItem()
    {
        GameObject obj = new GameObject();
        Item item = obj.AddComponent<Item>();

        Debug.Log(i);
        ItemData data = DataManager.instance.GetItemDataInfoById(i);
        i++;
        item.InputItemInfomationByID(data.id, data);
        obj.name = data.name;
        Sprite sprite = Resources.Load<Sprite>($"UI/Item/{data.spritename}");
        item.SetSprite(sprite);

        inven.GetItemTouch(item);

        
    }

}
