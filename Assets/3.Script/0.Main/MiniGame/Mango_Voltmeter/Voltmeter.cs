using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Voltmeter : MonoBehaviour
{
    [SerializeField] private Cylinder[] cylinderArray;
    [SerializeField] private VoltBtn[] btnArray;
    private float tolerance = 0.01f;

    public event Action Wrong;

    private void Start()
    {
        foreach (var cylinder in cylinderArray)
        {
            cylinder.AfterRotate += CheckAnswer;
        }

        Wrong += ChangeBtnTouchState;

    }

    public void RotateCylinder(VoltBtn btn)
    {
        for(int i = 0; i < 2; i ++)
        {
            cylinderArray[i].Rotate(btn.GetValue()[i]);
        }
    }

    private void CheckAnswer()
    {
        foreach(var cylinder in cylinderArray)
        {
            if (Mathf.Abs(cylinder.transform.eulerAngles.z - cylinder.correctValue) > tolerance)
            {
                Wrong?.Invoke();
                return; // `Wrong`이 호출되면 `Correct`를 호출하지 않음
            }
        }

        CompleteGame();
    }
    private void ChangeBtnTouchState()
    {
        foreach (var btn in btnArray)
        {
            if (btn.canTouch.Equals(false))
            {
                btn.canTouch = true;
                break;
            }
        }
    }

    private void CompleteGame()
    {
        //Debug.Log("게임 종료");
        foreach(var btn in btnArray)
        {
            btn.canTouch = false;
        }
    }
    
}
