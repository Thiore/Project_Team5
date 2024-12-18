using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eConnection
{
    Red = 0,
    Black
}

public class Connection : MonoBehaviour, ITouchable
{
    [SerializeField] private LayerMask touchableLayer;
    [SerializeField] private eConnection connectionColor;
    public eConnection getConnectionColor { get => connectionColor; }

    private Battery battery;
    public Battery getBattery { get => battery; }
    
    public bool isDrag;

    public Connection connectingObj;
    public LineRenderer line;

    private LineCollider childCollider;

    public bool isStart;

    private void Awake()
    {
        isDrag = false;
        TryGetComponent(out line);
        transform.parent.TryGetComponent(out battery);
        
        transform.GetChild(0).TryGetComponent(out childCollider);
        Debug.Log(line.endWidth);
        Debug.Log(line.startWidth);
        line.enabled = false;
        connectingObj = null;

    }
    public void OnTouchStarted(Vector2 position)
    {
        if(battery.isStart)
        {
            if (!isDrag)
            {
                if (connectingObj != null)
                {
                    if (connectingObj.line.enabled)
                    {
                        connectingObj.line.enabled = false;
                        connectingObj.transform.GetChild(0).gameObject.SetActive(false);
                        connectingObj.getBattery.ConnectingColor(connectingObj.getConnectionColor, false);
                    }

                    connectingObj.connectingObj = null;
                    connectingObj = null;
                }
                // LineRenderer 활성화
                line.enabled = true;
                battery.ConnectingColor(connectionColor, false);
                // LineRenderer의 시작 위치를 현재 오브젝트의 위치로 설정
                line.SetPosition(0, transform.position);
                line.SetPosition(1, transform.position);
                //LineRenderer의 충돌체 활성화
                childCollider.gameObject.SetActive(true);
                isDrag = true;// 드래그 상태 활성화
            }
        }
        
        
    }
    public void OnTouchHold(Vector2 position)
    {
        if(isDrag)
        {
            // 화면 좌표를 월드 좌표로 변환하여 Ray 생성
            Ray ray = Camera.main.ScreenPointToRay(position);
            RaycastHit hit;        // Ray가 맞은 대상 정보

            // Ray가 레이어 마스크에 맞는 오브젝트와 충돌하면
            if (Physics.Raycast(ray, out hit, TouchManager.Instance.getTouchDistance, touchableLayer,QueryTriggerInteraction.Ignore))
            {
                if(hit.collider.gameObject == gameObject)
                {
                    line.SetPosition(1, hit.point);
                    return;
                }
                int hitLayer = hit.collider.gameObject.layer;

                bool isTouchableObj = (touchableLayer.value & (1 << hitLayer)) != 0;
                if(isTouchableObj)
                {
                    if(hit.transform.TryGetComponent(out Connection connecting))
                    {
                        if(connecting.getConnectionColor.Equals(connectionColor))
                        {
                            line.SetPosition(1, hit.transform.position);
                        }
                    }
                    else
                    {
                        // LineRenderer의 끝 점 위치를 Ray 충돌 지점으로 설정
                        line.SetPosition(1, hit.point);
                    }
                    childCollider.SetCollider();
                    
                }
                
            }
        }
    }
    public void OnTouchEnd(Vector2 position)
    {
        if (isDrag)
        {
            // 화면 좌표를 월드 좌표로 변환하여 Ray 생성
            Ray ray = Camera.main.ScreenPointToRay(position);
            RaycastHit hit;        // Ray가 맞은 대상 정보

            // Ray가 레이어 마스크에 맞는 오브젝트와 충돌하면
            if (Physics.Raycast(ray, out hit, TouchManager.Instance.getTouchDistance, touchableLayer))
            {
                if (hit.transform.TryGetComponent(out Connection connecting))
                {
                    if (!connecting.getConnectionColor.Equals(connectionColor)&&connecting.connectingObj == null)
                    {
                        if (connecting.getBattery == battery)
                        {
                            line.enabled = false;
                            childCollider.gameObject.SetActive(false);
                            isDrag = false;
                            return;
                        }
                        line.SetPosition(1, hit.transform.position);
                        childCollider.SetCollider();
                        connectingObj = connecting;
                        connecting.connectingObj = this;
                        battery.ConnectingColor(connectionColor, true);
                        connecting.getBattery.ConnectingColor(connecting.getConnectionColor, true);
                        isDrag = false;
                        return;
                    }
                }
            }
            line.enabled = false;
            childCollider.gameObject.SetActive(false);
            isDrag = false;

        }
    }


    

   
}
