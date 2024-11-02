using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WiringPoint : MonoBehaviour
{
    private eColor color;
    private bool isSameColor = false;
    private bool isConnect;
    public bool IsConncet { get => isConnect; }

    public void SetboolConnect(bool isconnect)
    {
        isConnect = isconnect;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Wiring wiring))
        {
            wiring.SetBoolCheckColor(true);
            Debug.Log("연결성공");
            isSameColor = wiring.WiringSameColorCheck(color);

            if (isSameColor)
            {
                Debug.Log("같은 색 연결");
            }
            else
            {
                Debug.Log("다른 색 연결");
            }
            
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if(collision.gameObject.TryGetComponent(out Wiring wiring))
        {
            wiring.SetBoolCheckColor(false);
            Debug.Log("연결해제");
        }
    }

    public eColor GetWiringColor()
    {
        return color;
    }

    public void SetWiringColor(int color)
    {
        this.color = (eColor)color;
    }

    
}
