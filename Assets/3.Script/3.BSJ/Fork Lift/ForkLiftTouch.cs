using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ForkLiftTouch : MonoBehaviour, ITouchable
{
    [SerializeField] private LayerMask raycastLayerMask;
    [SerializeField] private BoxCollider myCol;
    [SerializeField] private int myIndex; // 개별 인덱스
    private Vector3 myPosition; // 자신의 초기 위치

    //[SerializeField] private 
    private BoxCollider palletCol;


    private void Awake()
    {
        TryGetComponent(out palletCol);
        palletCol.enabled = false;
    }


    private void Start()
    {
        myPosition = transform.position; // 초기 위치 저장
        
    }

    public void OnTouchStarted(Vector2 position)
    {
        if (myCol != null)
        {
            myCol.enabled = false; // 충돌 비활성화
        }
    }

    public void OnTouchHold(Vector2 position)
    {
        Ray ray = Camera.main.ScreenPointToRay(position);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, raycastLayerMask, QueryTriggerInteraction.Collide))
        {
            GameObject hitObj = hit.collider.gameObject;
            if (hitObj.layer == LayerMask.NameToLayer("Build"))
            {
                transform.position = hit.transform.position;
            }
            else if (hitObj.layer == LayerMask.NameToLayer("Area"))
            {
                transform.position = hit.point;
            }
        }
        else
        {
            Debug.Log("레이가 아무것도 맞추지 않았습니다.");
        }
    }

    public void OnTouchEnd(Vector2 position)
    {
        if (myCol != null)
        {
            myCol.enabled = true; // 충돌 복원
        }

        Ray ray = Camera.main.ScreenPointToRay(position);
        RaycastHit[] hits = Physics.RaycastAll(ray, 10f, raycastLayerMask);

        Debug.DrawRay(ray.origin, ray.direction * 20f, Color.red, 0.5f);

        if (hits.Length > 0)
        {
            // Array.Sort를 사용하여 거리 기준으로 정렬
            System.Array.Sort(hits, (a, b) => b.distance.CompareTo(a.distance));

            List<int> detectedIndices = new List<int>();
            ForkLiftCollect forkLiftCollect = null;

            foreach (RaycastHit hit in hits)
            {
                GameObject hitObj = hit.collider.gameObject;

                // ForkLiftTouch 처리
                ForkLiftTouch hitForkLift = hitObj.GetComponentInParent<ForkLiftTouch>();
                if (hitForkLift != null)
                {
                    detectedIndices.Add(hitForkLift.myIndex);
                    Debug.Log($"충돌한 오브젝트의 myIndex: {hitForkLift.myIndex}");
                }

                // ForkLiftCollect 검색
                if (forkLiftCollect == null)
                {
                    forkLiftCollect = hitObj.GetComponentInParent<ForkLiftCollect>();
                }
            }

            if (forkLiftCollect != null)
            {
                // 정렬된 인덱스 리스트를 ForkLiftCollect에 전달
                forkLiftCollect.CheckCompletion(detectedIndices, detectedIndices.Last());
            }
        }
        else
        {
            Debug.Log("아무것도 충돌하지 않음");
        }
    }

    public void MyPosition()
    {
        transform.position = myPosition;
    }
}
