using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSpin : MonoBehaviour
{
    public float rotationSpeed = 2f; // 회전 속도
    private Quaternion targetRotation; // 목표 회전값

    private void Start()
    {
        targetRotation = transform.rotation; // 현재 회전을 초기 목표값으로 설정
    }

    private void Update()
    {

    }

    // 90도 회전 설정
    private void Rotate()
    {
        targetRotation = Quaternion.Euler(0, transform.eulerAngles.y + 90, 0);
    }
}
