 using System.Collections;
 using System.Collections.Generic;
 using UnityEngine;
 using System;

public class VoltBtn : MonoBehaviour,ITouchable
{
    public bool isOn;
    public float[] onValue;
    public float[] offValue;
    private float onRotate = 300f;
    private float offRotate = 240f;
    public bool canTouch;

    [SerializeField] private Voltmeter voltMeter;
    
    //클릭 시 실행할 이벤트
    public event Action OnClick;

    private void Start()
    {
        isOn = false;

        canTouch = true;

        OnClick += BtnClick;
    }

    public void BtnClick()
    {
        ChangeBtnState();
        BtnRotate();
        ChangeTouchState();
        voltMeter.RotateCylinder(this);
    }
    
    private void ChangeBtnState()
    {
        isOn = !isOn;
    }
    public void ChangeTouchState()
    {
        canTouch = !canTouch;
    }
    private void BtnRotate()
    {
        if (isOn)
        {
            transform.localEulerAngles = new Vector3(onRotate, 90f,-90f);
        }
        else
        {
            transform.localEulerAngles = new Vector3(offRotate, 90f, -90f);
        }
    }
    public float[] GetValue()
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

    public void OnTouchStarted(Vector2 position)
    {
        if (canTouch) OnClick?.Invoke();

    }
    #region 안봐도됨
    public void OnTouchHold(Vector2 position)
    {
    }

    public void OnTouchEnd(Vector2 position)
    {
    }
    #endregion
}
