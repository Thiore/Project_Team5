using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_BtnInventory : MonoBehaviour
{
    [SerializeField] GameObject btnList;
    [SerializeField] GameObject inventory;

    public void OpenInventory()
    {
        for (int i = 0; i < btnList.transform.childCount; i++) 
        {
            btnList.transform.GetChild(i).gameObject.SetActive(false);        
        }
        inventory.SetActive(true);
    }

    public void CloseInventory()
    {
        for (int i = 0; i < btnList.transform.childCount; i++)
        {
            btnList.transform.GetChild(i).gameObject.SetActive(true);
        }
        inventory.SetActive(false);
    }
}
