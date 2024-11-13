using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using static UnityEditor.Progress;

public class TestBtn : MonoBehaviour
{
    [SerializeField] UI_Inventory inven;

    List<Item> list;

    int i = 0;

    private void Start()
    {
        list = FindObjectsOfType<Item>().ToList();
        Debug.Log(list.Count);
    }

    public void CreateItem()
    {

       // inven.GetItemTouch(list[i]);
        Debug.Log(list[i].name);
        i++;




    }

}
