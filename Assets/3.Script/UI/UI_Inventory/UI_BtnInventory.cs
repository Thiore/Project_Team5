using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_BtnInventory : MonoBehaviour
{
    [SerializeField] private GameObject btn;
    [SerializeField] private GameObject inventory;
    [SerializeField] private Toggle quickSlot;
    private Animator quickSlotAnim;

    private void Start()
    {
        quickSlot.TryGetComponent(out quickSlotAnim);
    }
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

    public void QuickSlot()
    {
        if(quickSlot.isOn)
        {
            quickSlotAnim.SetTrigger("Close");
        }
        else
        {
            quickSlotAnim.SetTrigger("Open");
        }
    }
}
