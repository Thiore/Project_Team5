using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideObject : MonoBehaviour, ITouchable
{
    private Vector3 initPosition;
    private Outline outLine;
    private Collider objectCollider; // 현재 오브젝트의 Collider
    public LayerMask overlapLayer;   // 충돌을 확인할 레이어 설정

    private Vector3 initialObjectPosition; // 터치 시작 시 오브젝트의 초기 위치
    private Vector2 initialTouchPosition;  // 터치 시작 시 손가락 위치

    [Header("SlidePuzzle최상위 오브젝트 추가")]
    [SerializeField] protected InteractionSlidePuzzle puzzleObj;

    private bool isSelected = true;

    [SerializeField] private int boxIndex;

    [Range(0.0001f,0.1f)]
    [SerializeField] private float moveSpeed;
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
        return IsOverlappingAtPosition(transform.position);
    }

    // 특정 위치에서 다른 오브젝트와 겹치는지 확인하는 메서드
    public bool IsOverlappingAtPosition(Vector3 position)
    {
        Collider[] overlappingColliders = null;
        switch (boxIndex)
        {
            case 0:
                Vector3 bigBoxSize = new Vector3(2.1f, 1f, 2.1f)*0.5f;
                bigBoxSize = Vector3.Scale(bigBoxSize, transform.lossyScale);
                // OverlapBox를 사용하여 특정 위치에서 겹침 여부 확인
                overlappingColliders = Physics.OverlapBox(
                    position,                      // 검사할 위치
                    bigBoxSize,                    // Collider 크기
                    transform.rotation,            // 회전 없음
                    overlapLayer                   // 충돌 확인할 레이어
                );
                break;
            case 1:
                Vector3 longBoxSize = new Vector3(2.1f, 1f, 1f) * 0.5f;
                longBoxSize = Vector3.Scale(longBoxSize, transform.lossyScale);
                // OverlapBox를 사용하여 특정 위치에서 겹침 여부 확인
                overlappingColliders = Physics.OverlapBox(
                    position,                      // 검사할 위치
                    longBoxSize,                   // Collider 크기
                    transform.rotation,            // 회전 없음
                    overlapLayer                   // 충돌 확인할 레이어
                );
                break;
            case 2:
                Vector3 smallBoxSize = new Vector3(1f, 1f, 1f) * 0.5f;
                smallBoxSize = Vector3.Scale(smallBoxSize, transform.lossyScale);
                // OverlapBox를 사용하여 특정 위치에서 겹침 여부 확인
                overlappingColliders = Physics.OverlapBox(
                    position,                      // 검사할 위치
                    smallBoxSize,                  // Collider 크기             
                    transform.rotation,            // 회전 없음
                    overlapLayer                   // 충돌 확인할 레이어
                );
                break;
            default:
                return true;
        }
        
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
            initialObjectPosition = transform.position; // 오브젝트의 초기 위치 저장
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

        // 이동할 X 오프셋 계산
        Vector3 moveOffset = CalculateMoveOffsetX(position);
        
        // X 방향으로 목표 위치 계산
        Vector3 targetPositionX = initialObjectPosition + transform.right*moveOffset.x;
        if (!IsOverlappingAtPosition(targetPositionX))
        {
            // 겹치지 않을 때만 X축 이동
            transform.position = targetPositionX;
            initialObjectPosition = targetPositionX; // 이동 후 초기 위치 업데이트
            //Debug.Log("X축 이동");
        }
        else
        {
            initialTouchPosition.x = position.x;
        }
    }

    private void MoveObjectWithTouchZ(Vector2 position)
    {

        // 이동할 Z 오프셋 계산
        Vector3 moveOffset = CalculateMoveOffsetZ(position);
        
        // Z 방향으로 목표 위치 계산
        Vector3 targetPositionZ = initialObjectPosition + transform.forward*moveOffset.z;

        
        if (!IsOverlappingAtPosition(targetPositionZ))
        {
            // 겹치지 않을 때만 Z축 이동
            transform.position = targetPositionZ;
            initialObjectPosition = targetPositionZ; // 이동 후 초기 위치 업데이트
            //Debug.Log("Z축 이동");
        }
        else
        {
            initialTouchPosition.y = position.y;
        }
    }

    private Vector3 CalculateMoveOffsetX(Vector2 currentTouchPosition)
    {
        Vector3 moveOffset = Vector3.zero;
        Vector2 touchDelta = currentTouchPosition - initialTouchPosition;
        // X 방향의 이동 설정
        if (!touchDelta.x.Equals(0f))
        {
            moveOffset.x = Mathf.Clamp(touchDelta.x, -1f, 1f) * moveSpeed;
            initialTouchPosition.x = currentTouchPosition.x; // 이동 후 새로운 기준점 설정
        }

        return moveOffset;
    }

    private Vector3 CalculateMoveOffsetZ(Vector2 currentTouchPosition)
    {
        Vector3 moveOffset = Vector3.zero;
        Vector2 touchDelta = currentTouchPosition - initialTouchPosition;
        // Z 방향의 이동 설정
        if (!touchDelta.y.Equals(0f))
        {
            moveOffset.z = Mathf.Clamp(touchDelta.y, -1f, 1f) * moveSpeed;
            
            initialTouchPosition.y = currentTouchPosition.y; // 이동 후 새로운 기준점 설정
        }

        return moveOffset;
    }
}
