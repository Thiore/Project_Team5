 using System.Collections;
 using System.Collections.Generic;
 using UnityEngine;
 using System;

public class OverlapPoint : MonoBehaviour, ITouchable
{
    public OverlapColor startColor;
    public Material lineMat;


    public LayerMask lineTouchLayer;

    public OverlapObj overlapObj;

    public List<OverlapObj> connectedObject;

    public event Action Check;


    private void Start()
    {
        overlapObj = GetComponent<OverlapObj>();
        overlapObj.line.material = lineMat;
    }




    public void OnTouchEnd(Vector2 position)
    {
        if (overlapObj.isConnected)
        {//연결이 된 경우
            foreach(var obj in connectedObject)
            {
                obj.isConnected = true;
            }
            Check?.Invoke();
        }
        else
        {
            foreach (var obj in connectedObject)
            {
                obj.isConnected = false;
                obj.line.enabled = false;
            }
            connectedObject.Clear();
        }
    }

    public void OnTouchHold(Vector2 position)
    {
        Ray ray = Camera.main.ScreenPointToRay(position);
        RaycastHit hit;
        // 레이캐스트를 통해 월드 좌표 계산
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, lineTouchLayer))
        {
            OverlapObj hitObject = hit.collider.GetComponent<OverlapObj>();

            string[] coordinate = hitObject.gameObject.name.Split(',');
            int x = 0;
            int.TryParse(coordinate[0] ,out x);
            int y;
            int.TryParse(coordinate[1], out y);

            if (overlapObj.isConnected)
            {
                return;
            }
            
            if(!hitObject.isPoint&& !hitObject.isConnected&&isAdjacent(x,y))
            {//포인트도 아니고 연결도 안된 경우 (빈 타일)
                if (connectedObject.Count >= 2)
                {
                    connectedObject[connectedObject.Count - 1].line.SetPosition(1, hitObject.linePos);
                }
                else
                {
                    overlapObj.line.SetPosition(1, hitObject.linePos);
                }
                hitObject.line.enabled = true;
                hitObject.line.material = lineMat;
                hitObject.line.SetPosition(0, hitObject.linePos);
                hitObject.line.SetPosition(1, hitObject.linePos);
                hitObject.connectColor = startColor;
                hitObject.isConnected = true;
                connectedObject.Add(hitObject);
            }
            else if (hitObject.isPoint && hitObject.connectColor.Equals(overlapObj.connectColor)&&!hitObject.Equals(overlapObj))
            {//같은 포인트인 경우 (시작/끝점)
                connectedObject[connectedObject.Count - 1].line.SetPosition(1, hitObject.linePos);
                overlapObj.isConnected = true;
                hitObject.isConnected = true;
                connectedObject.Add(hitObject);
                hitObject.GetComponent<OverlapPoint>().connectedObject = this.connectedObject;
                foreach(var obj in connectedObject)
                {
                    obj.isConnected = true;
                }

            }else if((hitObject.isPoint || hitObject.isConnected)&&!hitObject.connectColor.Equals(overlapObj.connectColor))
            {//다른 포인트이거나 다른 컬러와 연결된 경우
                //진동되면서 line disable
                foreach(var obj in connectedObject)
                {
                    obj.isConnected = false;
                    obj.line.enabled = false;
                }
                connectedObject.Clear();
                Handheld.Vibrate();

            }
            else if (hitObject.isConnected&&hitObject.connectColor.Equals(startColor)&&
                !connectedObject[connectedObject.Count-1].Equals(hitObject))
            {//현재 나와 연결되어있는 일반타일
                int index = connectedObject.IndexOf(hitObject);

                for (int i = connectedObject.Count - 1; i >= index; i--)
                {
                    connectedObject[i].line.enabled = false;
                    connectedObject[i].isConnected = false;
                    connectedObject.RemoveAt(i);
                }
            }else if (hitObject.Equals(overlapObj))
            {
                foreach(var obj in connectedObject)
                {
                    obj.line.enabled = false;
                }
                connectedObject.Clear();
                overlapObj.line.enabled = true;
                overlapObj.line.SetPosition(0, overlapObj.linePos);
                overlapObj.line.SetPosition(1, overlapObj.linePos);
                connectedObject.Add(overlapObj);
            }

        }

    }

    public void OnTouchStarted(Vector2 position)
    {
        if (overlapObj.isConnected)
        {
            foreach (var obj in connectedObject)
            {
                if (obj.GetComponent<OverlapPoint>() != null&&!obj.Equals(overlapObj)) obj.GetComponent<OverlapPoint>().connectedObject.Clear();

                obj.isConnected = false;
                obj.line.enabled = false;
            }
            connectedObject.Clear();
            overlapObj.line.enabled = true;
            overlapObj.line.SetPosition(0, overlapObj.linePos);
            overlapObj.line.SetPosition(1, overlapObj.linePos);
            connectedObject.Add(overlapObj);
        }
        else
        {
        Debug.Log($"{gameObject.name} 터치됨");
        overlapObj.line.enabled = true;
        overlapObj.line.SetPosition(0, overlapObj.linePos);
        overlapObj.line.SetPosition(1, overlapObj.linePos);
        connectedObject.Add(overlapObj);
        }

    }
    private bool isAdjacent(int x, int y)
    {
        // 오브젝트의 이름에서 좌표를 추출
        string[] coordinate = connectedObject[connectedObject.Count-1].name.Split(',');
        int myX = int.Parse(coordinate[0]);
        int myY = int.Parse(coordinate[1]);

        // 인접 여부를 판단하는 조건
        bool isAdjacentX = (myX == x && (myY == y + 1 || myY == y - 1)); // y 좌표가 ±1 차이
        bool isAdjacentY = (myY == y && (myX == x + 1 || myX == x - 1)); // x 좌표가 ±1 차이

        return isAdjacentX || isAdjacentY; // 둘 중 하나의 조건이라도 충족하면 true 반환
    }
}

public enum OverlapColor
{
    Blue,
    Gray,
    Pink,
    Red,
    Mint,
    Orange,
    Yellow,
    Purple,
    Null
}
