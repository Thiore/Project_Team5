using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ForkLiftTouch : MonoBehaviour, ITouchable
{
    [SerializeField] private LayerMask raycastLayerMask;
    [SerializeField] private BoxCollider myCol;

    public Stack<GameObject> buildStack = new Stack<GameObject>(); //부모의 스택
    private ForkLiftTouch currentParentForkLift; //현재 부모 오브젝트

    private void Start()
    {
        buildStack = new Stack<GameObject>();
    }

    public void OnTouchStarted(Vector2 position)
    {
        if (myCol != null)
        {
            myCol.enabled = false;
        }

        if (currentParentForkLift != null)
        {
            //이전 부모의 스택에서 제거
            currentParentForkLift.RemoveFromStack(gameObject);
            //부모 초기화
            currentParentForkLift = null;
        }
        //자신의 스택 비우기
        ClearStack();
    }

    public void OnTouchHold(Vector2 position)
    {
        Ray ray = Camera.main.ScreenPointToRay(position);
        RaycastHit hit;

        //Debug.DrawRay(ray.origin, ray.direction * 20f, Color.red, 0.5f);

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, raycastLayerMask))
        {
            ////충돌 오브젝트의 레이어 확인
            //if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Build"))
            //{
            //    //Ignore Raycast 레이어에 위치
            //    Vector3 upHit = hit.transform.position /*+ Vector3.up * 0.01f*/;
            //    transform.position = upHit;

            //    //currentParentForkLift = hit.collider.GetComponentInParent<ForkLiftTouch>();//부모 ForkLiftTouch 설정

            //    //부모가 이미 정해져 있다면, 그 부모를 따라가도록 설정
            //    if (currentParentForkLift == null)
            //    {
            //        currentParentForkLift = hit.collider.GetComponentInParent<ForkLiftTouch>();
            //    }
            //}
            //else if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Area"))
            //{
            //    //히트된 포지션에 오브젝트 위치
            //    transform.position = hit.point;

            //    //Area에서 이동했으므로 부모 없음
            //    currentParentForkLift = null; 

            //}

            GameObject hitObj = hit.collider.gameObject;

            if (hitObj.layer == LayerMask.NameToLayer("Build"))
            {
                //Ignore Raycast 레이어에 위치
                Vector3 upHit = hit.transform.position /*+ Vector3.up * 0.01f*/;
                transform.position = upHit;

                ForkLiftTouch newParentForkLift = hitObj.GetComponentInParent<ForkLiftTouch>();

                if (newParentForkLift != null && newParentForkLift != currentParentForkLift)
                {
                    // 중복 체크 및 새로운 부모의 스택에 추가
                    newParentForkLift.AddToStack(gameObject);
                    //부모 업데이트
                    currentParentForkLift = newParentForkLift;
                }
                else
                {
                    Debug.Log("새로운 부모 없음");
                }
            }
            else if (hitObj.layer == LayerMask.NameToLayer("Area"))
            {
                //히트된 포지션에 오브젝트 위치
                transform.position = hit.point;

                ClearStack();
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
            myCol.enabled = true; //충돌 복원
        }

        Ray ray = Camera.main.ScreenPointToRay(position);
        RaycastHit hit;

        //Debug.DrawRay(ray.origin, ray.direction * 20f, Color.red, 0.5f);

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, raycastLayerMask))
        {
            GameObject hitobj = hit.collider.gameObject;

            //Build 레이어 확인
            if (hitobj.layer == LayerMask.NameToLayer("Build"))
            {
                ////부모에서 ForkLiftTouch 컴포넌트 가져오기 
                //ForkLiftTouch parentForkLiftTouch = hitobj.transform.parent?.GetComponent<ForkLiftTouch>();
                //if (parentForkLiftTouch != null)
                //{
                //    parentForkLiftTouch.AddToStack(gameObject); //부모 스택에 추가
                //}
                //else
                //{
                //    Debug.Log("스택추가 안됨");
                //}

                //현재 오브젝트의 부모를 가져와서, 부모 스택에 추가
                if (currentParentForkLift != null)
                {
                    //부모 스택에 존재하지 않으면 추가
                    if (!currentParentForkLift.buildStack.Contains(gameObject))
                    {
                        currentParentForkLift.AddToStack(gameObject);

                    }
                    else
                    {
                        Debug.Log("이미 부모의 스택에 포함되어 있음");
                    }
                }
                else
                {
                    Debug.Log("스택 추가 안됨");
                }

            }
            else if (hitobj.layer == LayerMask.NameToLayer("Area"))
            {
                //다른 곳으로 이동했을 경우 스택 초기화
                ClearStack();
                Debug.Log("여기");
            }
        }
        else
        {
            Debug.Log("아무것도 충돌하지 않음");
        }
    }

    //스택에 오브젝트 추가
    public void AddToStack(GameObject obj)
    {
        //스택에 이미 추가되어 있다면 추가 하지 않음
        if (buildStack.Contains(obj))
        {
            Debug.Log($"{obj.name}은 이미 {name}의 스택에 포함되어 있음");
            return;
        }

        buildStack.Push(obj);

        //부모 - 자식 관계 설정
        obj.transform.parent = transform;
        Debug.Log($"{obj.name}이 {name}의 스택에 추가");

    }

    //스택 초기화 (다른 곳으로 이동했을 때)
    public void ClearStack()
    {
        while (buildStack.Count > 0)
        {
            GameObject child = buildStack.Pop();
            child.transform.parent = null; //부모 관계 해제
            Debug.Log($"{child.name}이 {name}의 스택에서 제거");
        }
    }

    public void RemoveFromStack(GameObject obj)
    {
        if (buildStack.Contains(obj))
        {
            Stack<GameObject> tempStack = new Stack<GameObject>();

            //임시 스택을 사용해 제거 대상만 제외
            while (buildStack.Count > 0)
            {
                GameObject top = buildStack.Pop();
                if (top != obj)
                {
                    tempStack.Push(top);
                }
            }

            //스택 복원
            while (tempStack.Count > 0)
            {
                buildStack.Push(tempStack.Pop());
            }
            //부모 관계 해제
            obj.transform.parent = null;
            Debug.Log($"{obj.name}이 {name}의 스택에서 제거됨");
        }
    }

}