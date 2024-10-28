using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UI_test : DefaultInputActions
{
    [SerializeField] GameObject target;
    [SerializeField] GameObject Exit;
    [SerializeField] GameObject item;

    public void ClickOn()
    {
        target.SetActive(true);
        Exit.SetActive(true);
        item.SetActive(true);
    }

    public void ClickOff()
    {
        target.SetActive(false);
        Exit.SetActive(false);
        item.SetActive(false);
    }

    
}
