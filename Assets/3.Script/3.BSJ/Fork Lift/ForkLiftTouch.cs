using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ForkLiftTouch : MonoBehaviour, ITouchable
{
    [SerializeField] private LayerMask raycastLayerMask;
    [SerializeField] private BoxCollider myCol;

    public Stack<GameObject> buildStack = new Stack<GameObject>(); //자신의 스택
    private ForkLiftTouch currentParentForkLift; //현재 오브젝트가 속한 ForkLiftTouch

    private void Start()
    {
        buildStack = new Stack<GameObject>();
    }

    public void OnTouchStarted(Vector2 position)
    {
        if(myCol !=null)
        {
            myCol.enabled = false;
        }

        ClearStack();
    }

    public void OnTouchHold(Vector2 position)
    {
        Ray ray = Camera.main.ScreenPointToRay(position);
        RaycastHit hit;

        //Debug.DrawRay(ray.origin, ray.direction * 20f, Color.red, 0.5f);

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, raycastLayerMask))
        {
            //충돌 오브젝트의 레이어 확인
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Build"))
            {
                //Ignore Raycast 레이어에 위치
                Vector3 upHit = hit.transform.position /*+ Vector3.up * 0.01f*/;
                transform.position = upHit;

                //currentParentForkLift = hit.collider.GetComponentInParent<ForkLiftTouch>();//부모 ForkLiftTouch 설정

                //부모가 이미 정해져 있다면, 그 부모를 따라가도록 설정
                if (currentParentForkLift == null)
                {
                    currentParentForkLift = hit.collider.GetComponentInParent<ForkLiftTouch>();
                }
            }
            else if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Area"))
            {
                //히트된 포지션에 오브젝트 위치
                transform.position = hit.point;

                //Area에서 이동했으므로 부모 없음
                currentParentForkLift = null; 
                
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
                    currentParentForkLift.AddToStack(gameObject);
                }
                else
                {
                    Debug.Log("스택 추가 안됨");
                }

            }
            else
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
    
}
