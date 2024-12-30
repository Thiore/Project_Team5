using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public enum WireColor
{
    Yellow,
    Green,
    Red,
    Orange
}
public interface ISetColor
{
    public void setColor(WireColor color);
    public void setMaterial();
}

public class StartPoint : MonoBehaviour, ITouchable,ISetColor
{
    public WireColor color;
    [SerializeField] private GameObject wire;
    [SerializeField] private LineRenderer line;
    [SerializeField] private Transform lineStartPoint;
    public float lineWidth;
    public LayerMask lineTouchLayer;


    public Material redMat;
    public Material yellowMat;
    public Material greenMat;
    public Material orangeMat;

    [SerializeField]private Renderer render;


    private Vector3 worldPosition; // 터치 위치의 월드 좌표를 저장할 변수
    private bool isTouching; // 터치 중인지 여부 확인
    private bool isConnected;
    private GameObject connectedObject;

    public bool isConnect;

    public event Action CheckConnecting;

    private void OnEnable()
    {
        line.startWidth=lineWidth;
        line.endWidth = lineWidth;
        line.enabled = false;
        isTouching = false;
    }

    public void OnTouchEnd(Vector2 position)
    {
        if(isTouching)
        {
            if (isConnected)
            {
                connectedObject.GetComponent<EndPoint>().isconnected = true;
                isConnect = true;
                CheckConnecting?.Invoke();
            }
            else
            {
                wire.SetActive(true);
                line.enabled = false;
                isConnect = false;
            }
        }
        isTouching = false;

       
    }

    public void OnTouchHold(Vector2 position)
    {
        if(isTouching)
        {
            Ray ray = Camera.main.ScreenPointToRay(position);
            RaycastHit hit;
            // 레이캐스트를 통해 월드 좌표 계산
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, lineTouchLayer))
            {
                worldPosition = hit.point; // 충돌한 지점의 좌표를 사용

                if (hit.collider.TryGetComponent(out EndPoint end))
                {
                    if (end.color.Equals(color))
                    {
                        line.SetPosition(1, hit.collider.GetComponent<EndPoint>().lineEndPostion.position);
                        this.isConnected = true;
                        connectedObject = hit.collider.gameObject;
                    }
                }
                else
                {
                    line.SetPosition(1, worldPosition);
                    this.isConnected = false;
                    connectedObject = null;
                }
            }
        }
        

    }

    public void OnTouchStarted(Vector2 position)
    {
        if(!isTouching)
        {
            wire.SetActive(false);
            line.enabled = true;
            line.SetPosition(0, lineStartPoint.position);
            line.SetPosition(1, lineStartPoint.position);
            isTouching = true;
        }
       
    }

    public void setColor(WireColor color)
    {
        this.color = color;
    }

    public void setMaterial()
    {
        if (render == null) return;// Debug.Log("render가 null");
        switch (color)
        {
            case WireColor.Yellow:
                render.material = yellowMat;
                break;
            case WireColor.Green:
                render.material = greenMat;
                break;
            case WireColor.Red:
                render.material = redMat;
                break;
            case WireColor.Orange:
                render.material = orangeMat;
                break;
        }
    }
}
