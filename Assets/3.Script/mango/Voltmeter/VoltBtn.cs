using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoltBtn : MonoBehaviour
{
    public bool isOn;
    public int[] onValue = new int[2];
    public int[] offValue = new int[2];
    private float onRotate = 300;
    private float offRotate = 240;


    private void Start()
    {
        isOn = false;
    }
    private void Update()
    {
        
    }

    public void BtnClick()
    {
        ChangeState();
        BtnRotate();
    }
    
    private void ChangeState()
    {
        isOn = !isOn;
    }
    private void BtnRotate()
    {
        if (isOn)
        {
            transform.Rotate(onRotate, transform.rotation.y, transform.rotation.z);
        }
        else
        {
            transform.Rotate(offRotate, transform.rotation.y, transform.rotation.z);
        }
    }
    public int[] GetValue()
    {
        if (isOn)
        {
            return onValue;
        }
        else
        {
            return offValue;
        }
    }

}
