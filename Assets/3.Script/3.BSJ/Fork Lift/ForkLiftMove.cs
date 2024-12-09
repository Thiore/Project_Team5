using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ForkLiftMove : MonoBehaviour
{
    private Vector2 moveInput;
    private Vector3 moveDir;
    [SerializeField] private Camera playerCamera;
    [Range(1, 10)]
    [SerializeField] private float speed;

    private Rigidbody rid;
    private void Start()
    {
        moveInput = Vector2.zero;
        moveDir = Vector3.zero;
        rid = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        Vector3 cameraFoward = playerCamera.transform.forward;
        cameraFoward.y = 0f;
        cameraFoward.Normalize();

        Vector3 cameraRight = playerCamera.transform.right;
        cameraRight.y = 0f;
        cameraRight.Normalize();
        moveDir = (cameraRight * moveInput.x + cameraFoward * moveInput.y).normalized;
    }
    private void FixedUpdate()
    {
        //�̸� ���س��� ���⺤�ͷ� �÷��̾� �̵�
        transform.Translate(moveDir * speed * Time.fixedDeltaTime);

    }
    private void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }
}
