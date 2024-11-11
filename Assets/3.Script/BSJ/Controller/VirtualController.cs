using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirtualController : MonoBehaviour,ITouchable
{
    private bool isvirtualOff = true;

    // 시네머신 버추얼 카메라
    [SerializeField] private GameObject virtualCam;

    public void OnTouchEnd(Vector2 position)
    {
        
    }

    public void OnTouchHold(Vector2 position)
    {
        
    }

    public void OnTouchStarted(Vector2 position)
    {
        ObjTouch();
    }

    private void ObjTouch()
    {
        if (isvirtualOff)
        {
            virtualCam.gameObject.SetActive(true);
            isvirtualOff = false;
        }
        else if (virtualCam.activeSelf && !isvirtualOff)
        {
            virtualCam.gameObject.SetActive(false);
            isvirtualOff = true;
        }
    }
}
