using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    private _InputManager input;
    private Rigidbody rb;

    public float speed;

    Vector3 moveDir;

    private void Awake()
    {
        input = GetComponent<_InputManager>();
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        // 이동 방향을 입력 값으로 설정 (로컬 기준으로 변환하기 전)
        moveDir = new Vector3(input.MoveInput.x, 0, input.MoveInput.y).normalized;
    }

    private void FixedUpdate()
    {
        if (rb != null)
        {
            // 이동 방향을 로컬 좌표계로 변환
            Vector3 localMoveDir = transform.TransformDirection(moveDir);

            // 로컬 좌표계 기준으로 이동 적용
            rb.MovePosition(rb.position + localMoveDir * speed * Time.fixedDeltaTime);
        }
    }
}
