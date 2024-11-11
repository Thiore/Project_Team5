using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestQucik : MonoBehaviour
{
    private Toggle btn;

    private void Start()
    {
        TryGetComponent(out btn);
    }
    public void test()
    {
        if(btn.isOn)
        {
            gameObject.SetActive(true);
        }
        if(!btn.isOn)
        {
            gameObject.SetActive(false);
        }
    }
}
