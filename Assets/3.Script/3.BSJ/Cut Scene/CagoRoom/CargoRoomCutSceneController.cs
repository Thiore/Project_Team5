using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using  Cinemachine;

public class CargoRoomCutSceneController : MonoBehaviour
{
    [SerializeField] private CinemachineBrain cinemachineBrain; //Blend 변경을 위한 참조
    [SerializeField] private GameObject palletPuzzle;
    [SerializeField] private GameObject settingStair;


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

    public void GameEnd()
    {
        if (PlayerManager.Instance != null)
        {
            PlayerManager.Instance.SetBtn(true);
        }
        TouchManager.Instance.EnableMoveHandler(true);
        palletPuzzle.SetActive(false);
        settingStair.SetActive(true);
    }
}
