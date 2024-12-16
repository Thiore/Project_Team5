using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirtualBlendTime : MonoBehaviour
{
    [SerializeField] private float blendTime;
    private void OnEnable()
    {
        GameManager.Instance.CurrentFrameChangeBlendTime(blendTime);
    }
    private void OnDisable()
    {
        GameManager.Instance.ResetBlendTime();
    }
}
