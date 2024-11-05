using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_BtnInventory : MonoBehaviour
{
    [SerializeField] GameObject btn;
    [SerializeField] GameObject inventory;

    public void OpenInventory()
    {
        
            btn.SetActive(false);        
        
        inventory.SetActive(true);
    }

    public void CloseInventory()
    {
        
            btn.SetActive(true);
        
        inventory.SetActive(false);
    }
}
