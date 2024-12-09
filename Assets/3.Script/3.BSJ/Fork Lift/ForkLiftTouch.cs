using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ForkLiftTouch : MonoBehaviour, ITouchable
{
    [SerializeField] private LayerMask raycastLayerMask;
    [SerializeField] private BoxCollider myCol;

    public void OnTouchStarted(Vector2 position)
    {
        if(myCol !=null)
        {
            myCol.enabled = false;
        }
    }

    public void OnTouchHold(Vector2 position)
    {
        Ray ray = Camera.main.ScreenPointToRay(position);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, raycastLayerMask))
        {
            //충돌 오브젝트의 레이어 확인
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Build"))
            {
                //Ignore Raycast 레이어에 위치
                Vector3 upHit = hit.transform.position /*+ Vector3.up * 0.01f*/;
                transform.position = upHit;
                Debug.Log("Ignore Raycast 레이어 위에 오브젝트 배치");
            }
            else if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Area"))
            {
                //히트된 포지션에 오브젝트 위치
                transform.position = hit.point;
                Debug.Log("히트된 위치로 오브젝트 이동");
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
            myCol.enabled = true;
        }
    }
}
