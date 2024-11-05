using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    private InputManager input; // input���� ������ �Ŵ���
    
    [SerializeField] private Transform playerCamera; //�÷��̾ ������ ������ ī�޶�

    
    [SerializeField] private float speed; //�÷��̾��� �ӵ�

    
    Vector3 moveDir; //�÷��̾ �̵��� ���⺤��

    private void Start()
    {
        input = InputManager.Instance;
    }

    private void Update()
    {

        Vector3 cameraFoward = playerCamera.transform.forward;
        cameraFoward.y = 0f;
        cameraFoward.Normalize();

        Vector3 cameraRight = playerCamera.transform.right;
        cameraRight.y = 0f;
        cameraRight.Normalize();

        // �̵� ������ �ʱ� ��ġ ��ǥ�� ���� �Է� ��ǥ�� ���
        Vector2 joystickInput = input.moveData.value - input.moveData.startValue;
        //���� ��ǥ�� �̵��ؾ��� ���⺤�� ����
        moveDir = (cameraRight * joystickInput.x + cameraFoward * joystickInput.y).normalized;


        
    }

    private void FixedUpdate()
    {
        //�̸� ���س��� ���⺤�ͷ� �÷��̾� �̵�
        transform.Translate(moveDir * speed * Time.fixedDeltaTime);
        
    }
}
