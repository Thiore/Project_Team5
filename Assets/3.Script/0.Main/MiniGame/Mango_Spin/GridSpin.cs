using UnityEngine;
using System;
using System.Collections;

public class GridSpin : MonoBehaviour, ITouchable
{
    public float rotationSpeed = 2f; // 회전 속도
    private Vector3 targetRotation; // 목표 회전값

    // 회전 완료 시 실행할 이벤트
    public event Action OnRotationComplete;

    private Coroutine rotate_co = null; // 회전 중 여부 확인

    private void Start()
    {
        targetRotation = transform.localEulerAngles; // 현재 회전을 초기 목표값으로 설정
    }

    private IEnumerator Rotate_co()
    {
        float rotationTime = 0f;

        Vector3 curRotation = transform.localEulerAngles;

        // 목표 회전으로 부드럽게 회전
        while(true)
        {
            rotationTime += Time.deltaTime * rotationSpeed;
            transform.localEulerAngles = Vector3.Lerp(curRotation, targetRotation, Mathf.Clamp(rotationTime,0f,1f));
            if (rotationTime >= 1f)
            {
                // 목표 회전에 근사하면 정확히 목표 회전으로 설정
                transform.localEulerAngles = targetRotation;
               
                
                break;
            }
            yield return null;
        }
        // 회전 완료 시 이벤트 호출
        OnRotationComplete?.Invoke();
        rotate_co = null;
        yield break;

        

    }

    // 90도 회전 설정
    private void Rotate()
    {
        targetRotation = transform.localEulerAngles + new Vector3(0f, 90f, 0f);
    }

    public void OnTouchStarted(Vector2 position)
    {
    }

    public void OnTouchHold(Vector2 position)
    {
    }

    public void OnTouchEnd(Vector2 position)
    {
        // 회전 중이 아닐 때만 터치 감지
        if (rotate_co == null)
        {
            Ray ray = Camera.main.ScreenPointToRay(position);
            if (Physics.Raycast(ray, out RaycastHit hit, TouchManager.Instance.getTouchDistance, TouchManager.Instance.getTouchableLayer))
            {
                if (hit.collider.gameObject.Equals(gameObject))
                {
                    Rotate();
                    rotate_co = StartCoroutine(Rotate_co());
                }
                    
            }
                
        }
    }
}
