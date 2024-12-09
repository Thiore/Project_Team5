using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideObject : MonoBehaviour
{
    private Vector3 initPosition;
    private Outline outLine;
    private Collider objectCollider; // 현재 오브젝트의 Collider
    public LayerMask overlapLayer;   // 충돌을 확인할 레이어 설정

    private Vector3 initialObjectPosition; // 터치 시작 시 오브젝트의 초기 위치
    private Vector2 initialTouchPosition;  // 터치 시작 시 손가락 위치

   

    [Header("결과와 상호작용하는 오브젝트는 true와 correctZone오브젝트 추가")]
    [SerializeField] private bool isCheckObj;
    [SerializeField] private CorrectCheck correctZone;

    protected virtual void Awake()
    {
        initPosition = transform.position;
        TryGetComponent(out outLine);
        outLine.enabled = false;
        TryGetComponent(out objectCollider);

    }
    

    // 현재 위치에서 다른 오브젝트와 겹치는지 확인하는 메서드
    public bool IsOverlapping()
    {
        return IsOverlappingAtPosition(objectCollider.bounds.center);
    }

    // 특정 위치에서 다른 오브젝트와 겹치는지 확인하는 메서드
    public bool IsOverlappingAtPosition(Vector3 position)
    {
        if (objectCollider == null) return false;

        // OverlapBox를 사용하여 특정 위치에서 겹침 여부 확인
        Collider[] overlappingColliders = Physics.OverlapBox(
            position,                      // 검사할 위치
            objectCollider.bounds.extents, // Collider 크기
            Quaternion.identity,           // 회전 없음
            overlapLayer                   // 충돌 확인할 레이어
        );

        // 겹친 Collider가 있는지 확인 (자기 자신은 제외)
        foreach (Collider collider in overlappingColliders)
        {
            if (collider.gameObject != gameObject)
            {
                return true; // 다른 오브젝트와 겹침
            }
        }
        return false; // 겹친 오브젝트가 없음
    }

    public void InitPosition()
    {
        transform.position = initPosition;
    }
}
