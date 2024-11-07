using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LockGame : MonoBehaviour
{
    //상호작용 관련
    [SerializeField] private int floorIndex; //오브젝트의 현재 층
    [SerializeField] private int objectIndex; //오브젝트 본인의 인덱스
    private SaveManager saveManager; //상태관리

    private int[] correctNumber = { 1, 3, 0, 4 }; //정답 번호
    private int[] currentNumber = { 0, 0, 0, 0 }; //현재 번호

    //각 번호 휠 오브젝트 (4개)
    public Transform[] numberWheels;

    public Animator ani;

    //회전 각도 설정
    private float rotationAngle = -36f;

    //회전 속도
    private float rotationSpeed = 5f;

    //각 휠의 목표 회전 각도
    private Quaternion[] targetRotations;

    public Camera mainCamera;

    public GameObject camera1;
    public GameObject camera2;
    public GameObject canvas;
    private ReadInputData input;


    private void Start()
    {
        saveManager = GameObject.FindGameObjectWithTag("SaveManager").GetComponent<SaveManager>();

        TryGetComponent(out input);

        ani.GetComponent<Animator>();

        //번호 리셋
        ResetLock();

        //초기 목표 회전 각도
        targetRotations = new Quaternion[numberWheels.Length];
        for (int i = 0; i < numberWheels.Length; i++)
        {
            targetRotations[i] = numberWheels[i].localRotation;
        }
    }
    private void Update()
    {
        GameSetting();
        for (int i = 0; i < numberWheels.Length; i++)
        {
            //목표 회전 각도까지 부드럽게
            numberWheels[i].localRotation = Quaternion.Lerp
                (numberWheels[i].localRotation,
                targetRotations[i],
                Time.deltaTime * rotationSpeed);

        }
    }

    //번호 리셋
    public void ResetLock()
    {
        for (int i = 0; i < currentNumber.Length; i++)
        {
            currentNumber[i] = 0;
            numberWheels[i].localRotation = Quaternion.Euler(0, 0, -180);
        }
    }

    

    //특정 번호 휠을 오른쪽으로 회전(+36도)
    public void RotateWheelRight(int wheelIndext)
    {
        currentNumber[wheelIndext] = (currentNumber[wheelIndext] + 1) % 10;

        //각 번호 휠을 해당 숫자에 맞게 회전
        float newRotation = currentNumber[wheelIndext] * rotationAngle;
        targetRotations[wheelIndext] = Quaternion.Euler(0, newRotation, -180);

        //정답확인
        CheckNumber();
    }

    //특정 번호 휠을 왼쪽으로 회전 (-36도)
    public void RotateWheelLeft(int wheelIndex)
    {
        //숫자를 감소시키고, 0보다 작으면 9로 설정
        currentNumber[wheelIndex] = (currentNumber[wheelIndex] - 1 + 10) % 10;

        //각 번호 휠을 해당 숫자에 맞게 회전
        float newRotation = currentNumber[wheelIndex] * rotationAngle;
        targetRotations[wheelIndex] = Quaternion.Euler(0, newRotation, -180);

        //정답 확인
        CheckNumber();
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
        saveManager.UpdateObjectState(floorIndex, objectIndex, true);

        //번호가 맞으면 자물쇠 열리는 애니메이션 실행
        LockOpenAnimation();
    }

    private void LockOpenAnimation()
    {
        EndGame();
        ani.SetTrigger("Open");
        Debug.Log("정답");
    }

    private void GameSetting()
    {
        if (input.isTouch)
        {
            camera1.gameObject.SetActive(true);
            canvas.gameObject.SetActive(true);
        }
    }

    private void EndGame()
    {
        camera1.gameObject.SetActive(false);
        canvas.gameObject.SetActive(false);
        camera2.gameObject.SetActive(true);
    }
}
