using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WiringPoint : MonoBehaviour
{
    private eColor color;
    private bool isConnect;
    public bool IsConncet { get => isConnect; set => isConnect = value; }



    private void OnCollisionExit2D(Collision2D collision)
    {
        if(collision.gameObject.TryGetComponent(out Wiring wiring))
        {
            wiring.IsSameColor = false;
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
