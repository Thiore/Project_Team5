using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    private InputManager input; // input값을 참조할 매니저
    
    [SerializeField] private Camera playerCamera; //플레이어가 방향을 참조할 카메라

    
    [SerializeField] private float speed; //플레이어의 속도

    
    Vector3 moveDir; //플레이어가 이동할 방향벡터

    private void Awake()
    {
        input = InputManager.Instance;
    }

    private void Update()
    {
        //플레이어오브젝트가 바라볼 방향 설정
        //Vector3 cameraRot = new Vector3(0, playerCamera.transform.localEulerAngles.y, 0);
        //transform.eulerAngles = cameraRot;
        // 이동 방향을 초기 터치 좌표와 현재 입력 좌표로 계산
        Vector2 joystickInput = input.moveData.value - input.moveData.startValue;
        //계산된 좌표로 이동해야할 방향벡터 설정
        moveDir = new Vector3(joystickInput.x, 0, joystickInput.y).normalized;

        
    }

    private void FixedUpdate()
    {
        //미리 구해놓은 방향벡터로 플레이어 이동
        transform.Translate(moveDir * speed * Time.fixedDeltaTime);
        
    }
}
