using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using  Cinemachine;

public class CameraBlendController : MonoBehaviour
{
    [SerializeField] private CinemachineBrain cinemachineBrain; //Blend 변경을 위한 참조


    // Blend 시간 0초
    public void BlendTimeStart()
    {
        cinemachineBrain.m_DefaultBlend.m_Time = 0;
    }

    // Blend 시간 기본 값으로 설정
    public void BlendTimeDefault()
    {
        cinemachineBrain.m_DefaultBlend.m_Time = 2;
    }
}
