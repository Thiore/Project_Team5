using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideObject : MonoBehaviour, ITouchable
{
    private Vector3 initPosition;
    private Outline outLine;
    private Collider objectCollider; // 현재 오브젝트의 Collider
    public LayerMask overlapLayer;   // 충돌을 확인할 레이어 설정

    //private Vector3 initialObjectPosition; // 터치 시작 시 오브젝트의 초기 위치
    private Vector2 initialTouchPosition;  // 터치 시작 시 손가락 위치
    private Vector3 targetPosition;

    [Header("SlidePuzzle최상위 오브젝트 추가")]
    [SerializeField] protected InteractionSlidePuzzle puzzleObj;

    private bool isSelected = true;
   
    protected virtual void Awake()
    {
        initPosition = transform.position;
        targetPosition = transform.position;
        TryGetComponent(out outLine);
        outLine.enabled = false;
        TryGetComponent(out objectCollider);

    }
    

    // 현재 위치에서 다른 오브젝트와 겹치는지 확인하는 메서드
    public bool IsOverlapping()
    {
        return IsOverlappingAtPosition(transform.position);
    }

    // 특정 위치에서 다른 오브젝트와 겹치는지 확인하는 메서드
    public bool IsOverlappingAtPosition(Vector3 position)
    {
        if (objectCollider == null) return false;

        // OverlapBox를 사용하여 특정 위치에서 겹침 여부 확인
        Collider[] overlappingColliders = Physics.OverlapBox(
            position,                      // 검사할 위치
            objectCollider.bounds.extents*0.5f, // Collider 크기
            Quaternion.identity           // 회전 없음
            //overlapLayer                   // 충돌 확인할 레이어
        );
        
        // 겹친 Collider가 있는지 확인 (자기 자신은 제외)
        foreach (Collider collider in overlappingColliders)
        {
            Debug.Log("col" + collider.gameObject);
            Debug.Log("game" + gameObject);
            if (collider.gameObject != gameObject)
            {
                return true; // 다른 오브젝트와 겹침
            }
        }
        return false; // 겹친 오브젝트가 없음
    }


    /// <summary>
    /// 초기화시에 퍼즐 위치 초기화
    /// </summary>
    public void InitPosition()
    {
        transform.position = initPosition;
    }

    public void OnTouchStarted(Vector2 position)
    {
        if(puzzleObj.SetSelectObj(gameObject))
        {
            isSelected = true;
            outLine.enabled = true;
            initialTouchPosition = position; // 터치 시작 위치 저장
            //initialObjectPosition = transform.localPosition; // 오브젝트의 초기 위치 저장
        }
        else
        {
            isSelected = false;
        }
    }

    public void OnTouchHold(Vector2 position)
    {
        if(isSelected)
        {
            // 오브젝트가 선택된 상태에서 손가락 이동에 따라 오브젝트를 0.5 단위로 이동
            MoveObjectWithTouchX(position);
            MoveObjectWithTouchZ(position);
        }
    }

    public virtual void OnTouchEnd(Vector2 position)
    {
        if(isSelected)
        {
            outLine.enabled = false;
            puzzleObj.ResetSelectObj();
            isSelected = false;
        }
    }
    private void MoveObjectWithTouchX(Vector2 position)
    {
        // 현재 터치 위치 가져오기
        Vector2 currentTouchPosition = position;

        // 이동할 X 오프셋 계산
        Vector3 moveOffset = CalculateMoveOffsetX(currentTouchPosition);

        // X 방향으로 목표 위치 계산
        targetPosition = transform.position - transform.right*moveOffset.x;

        if (IsOverlappingAtPosition(targetPosition))
        {
            // 겹치지 않을 때만 X축 이동
            transform.position = targetPosition;
            //initialObjectPosition = targetPositionX; // 이동 후 초기 위치 업데이트
            //Debug.Log("X축 이동");
        }
        else
        {
            initialTouchPosition.y = currentTouchPosition.y;
        }
    }

    private void MoveObjectWithTouchZ(Vector2 position)
    {

        // 현재 터치 위치 가져오기
        Vector2 currentTouchPosition = position;

        // 이동할 Z 오프셋 계산
        Vector3 moveOffset = CalculateMoveOffsetZ(currentTouchPosition);

        // Z 방향으로 목표 위치 계산
        targetPosition = transform.position - transform.forward*moveOffset.z;

        
        if (IsOverlappingAtPosition(targetPosition))
        {
            // 겹치지 않을 때만 Z축 이동
            transform.position = targetPosition;
            //initialObjectPosition = targetPositionZ; // 이동 후 초기 위치 업데이트
            //Debug.Log("Z축 이동");
        }
        else
        {
            initialTouchPosition.x = currentTouchPosition.x;
        }
    }

    private Vector3 CalculateMoveOffsetX(Vector2 currentTouchPosition)
    {
        Vector3 moveOffset = Vector3.zero;
        Vector2 touchDelta = currentTouchPosition - initialTouchPosition;

        // X 방향의 이동 설정
        if (Mathf.Abs(touchDelta.y) >= 0.1f)
        {
            moveOffset.x = -Mathf.Sign(touchDelta.y) * 0.25f;
            initialTouchPosition.y = currentTouchPosition.y; // 이동 후 새로운 기준점 설정
        }

        return moveOffset;
    }

    private Vector3 CalculateMoveOffsetZ(Vector2 currentTouchPosition)
    {
        Vector3 moveOffset = Vector3.zero;
        Vector2 touchDelta = currentTouchPosition - initialTouchPosition;

        // Z 방향의 이동 설정
        if (Mathf.Abs(touchDelta.x) >= 0.1f)
        {
            moveOffset.z = Mathf.Sign(touchDelta.x) * 0.25f;
            initialTouchPosition.x = currentTouchPosition.x; // 이동 후 새로운 기준점 설정
        }

        return moveOffset;
    }
}
