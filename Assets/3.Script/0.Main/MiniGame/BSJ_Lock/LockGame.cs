using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LockGame : MonoBehaviour
{

    private int[] correctNumber = { 1, 3, 0, 4 }; //정답 번호
    private int[] currentNumber = { 0, 0, 0, 0 }; //현재 번호

    //회전중인 휠 코루틴
    private Coroutine[] rotation_co;

    //각 번호 휠 오브젝트 (4개)
    public Transform[] numberWheels;

    public Animator ani;

    //회전 각도 설정
    private float rotationAngle = -36f;

    //회전 속도
    private float rotationSpeed = 5f;

    //각 휠의 목표 회전 각도
    private Quaternion[] targetRotations;


    
    public GameObject canvas;
    public bool isAnswer;


    private void Start()
    {

        ani.GetComponent<Animator>();

        //초기 목표 회전 각도
        targetRotations = new Quaternion[numberWheels.Length];
        for (int i = 0; i < numberWheels.Length; i++)
        {
            targetRotations[i] = numberWheels[i].localRotation;
        }

        //회전코루틴 초기화
        rotation_co = new Coroutine[4];
        for(int i = 0; i < 4;i++)
        {
            rotation_co[i] = null;
        }

        //번호 리셋
        ResetLock();
    }
    //private void Update()
    //{
    //    //GameSetting();
    //    for (int i = 0; i < numberWheels.Length; i++)
    //    {
    //        //목표 회전 각도까지 부드럽게
    //        numberWheels[i].localRotation = Quaternion.Lerp
    //            (numberWheels[i].localRotation,
    //            targetRotations[i],
    //            Time.deltaTime * rotationSpeed);

    //    }
    //}

    //번호 리셋
    public void ResetLock()
    {
        for (int i = 0; i < currentNumber.Length; i++)
        {
            if(rotation_co[i] != null)
            {
                StopCoroutine(rotation_co[i]);
                rotation_co[i] = null;
            }
            
            currentNumber[i] = 0;
            numberWheels[i].localRotation = Quaternion.Euler(0f, 0f, -180f);
        }
    }

    

    //특정 번호 휠을 오른쪽으로 회전(+36도)
    public void RotateWheelRight(int wheelIndex)
    {
        if(rotation_co[wheelIndex] == null && !isAnswer)
        {
            currentNumber[wheelIndex] = (currentNumber[wheelIndex] + 1) % 10;

            //각 번호 휠을 해당 숫자에 맞게 회전
            float newRotation = currentNumber[wheelIndex] * rotationAngle;
            targetRotations[wheelIndex] = Quaternion.Euler(0, newRotation, -180);
            rotation_co[wheelIndex] = StartCoroutine(RotateWheel(wheelIndex));
        }
    }

    //특정 번호 휠을 왼쪽으로 회전 (-36도)
    public void RotateWheelLeft(int wheelIndex)
    {
        if (rotation_co[wheelIndex] == null&&!isAnswer)
        {
            //숫자를 감소시키고, 0보다 작으면 9로 설정
            currentNumber[wheelIndex] = (currentNumber[wheelIndex] - 1 + 10) % 10;

            //각 번호 휠을 해당 숫자에 맞게 회전
            float newRotation = currentNumber[wheelIndex] * rotationAngle;
            targetRotations[wheelIndex] = Quaternion.Euler(0, newRotation, -180);
            rotation_co[wheelIndex] = StartCoroutine(RotateWheel(wheelIndex));
        }
    }

    /// <summary>
    /// 코루틴 회전을 위한 변수입니다.
    /// </summary>
    /// <param name="wheelIndex">선택된 휠의 index값</param>
    /// <returns></returns>
    private IEnumerator RotateWheel(int wheelIndex)
    {
        float rotationTime = 0f;

        while(rotationTime/rotationSpeed<1f)
        {
            //Debug.Log(rotationTime);
            rotationTime += Time.deltaTime;
            //목표 회전 각도까지 부드럽게
            numberWheels[wheelIndex].localRotation =
                Quaternion.Lerp(numberWheels[wheelIndex].localRotation,
                                 targetRotations[wheelIndex],
                                 rotationTime / rotationSpeed);
            yield return null;
        }
        //정답 확인
        CheckNumber();
        rotation_co = null;
        yield break;

    }

    //현재 번호와 정답 번호 비교
    private void CheckNumber()
    {
        for (int i = 0; i < correctNumber.Length; i++)
        {
            if (currentNumber[i] != correctNumber[i])
                return; //하나라도 틀리면 반환
        }

        //번호가 맞으면 상태(true) 저장
        //SaveManager.Instance.UpdateObjectState(floorIndex, objectIndex[0], true);
        isAnswer = true;
        canvas.gameObject.SetActive(false);
        //번호가 맞으면 자물쇠 열리는 애니메이션 실행
        //puzzle.OffInteraction();
    }


    //private void GameSetting()
    //{
    //    if (input.isTouch)
    //    {
    //        camera1.gameObject.SetActive(true);
    //        canvas.gameObject.SetActive(true);
    //    }
    //}

    //private void EndGame()
    //{
    //    camera1.gameObject.SetActive(false);
    //    canvas.gameObject.SetActive(false);
    //    camera2.gameObject.SetActive(true);
    //}
}
