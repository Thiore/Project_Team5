using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirtualController : MonoBehaviour
{
    private ReadInputData input; //isTouch
    private bool isvirtualOff = true;

    // 시네머신 버추얼 카메라
    [SerializeField] private GameObject virtualCam;

    private void Start()
    {
        TryGetComponent(out input);
    }

    private void Update()
    {
        if (input.isTouch && isvirtualOff)
        {
            virtualCam.gameObject.SetActive(true);
            isvirtualOff = false;
        }
        else if (virtualCam.activeSelf && input.isTouch && !isvirtualOff)
        {
            virtualCam.gameObject.SetActive(false);
            isvirtualOff = true;
        }


    }
}
