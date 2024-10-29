using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionCamera : MonoBehaviour
{
    [Header("미션 어태치")]
    [SerializeField] private GameObject[] mission = null; // 미션을 담고 있는 게임 오브젝트 배열

    [Header("프로퍼티")]
    [SerializeField] private float MissionSentenceOnDelay = 0; // 미션 내용 활성화 지연 시간

    private int delay_index = 0; // 현재 활성화할 미션의 인덱스

    public void Delay() // 미션 내용 활성화 함수
    {
        // 현재 미션의 자식 오브젝트들을 활성화
        for (int i = 1; i < mission[delay_index].transform.childCount; i++)
        {
            mission[delay_index].transform.GetChild(i).gameObject.SetActive(true); // 미션 활성화
        }
    }

    public void Mission(int index) // 미션 카메라 ON 함수
    {
        delay_index = index; // 전달받은 인덱스를 저장 (딜레이 함수에 사용)

        mission[delay_index].transform.GetChild(0).gameObject.SetActive(true); // 미션 카메라 활성화

        Invoke("Delay", MissionSentenceOnDelay); // 미션 내용 활성화 지연 호출
    }

    public void Close(int index) // 미션 카메라 OFF 함수
    {
        // 현재 미션의 모든 자식 오브젝트들을 비활성화
        for (int i = 0; i < mission[index].transform.childCount; i++)
        {
            mission[index].transform.GetChild(i).gameObject.SetActive(false); // 미션 비활성화
        }
    }
}