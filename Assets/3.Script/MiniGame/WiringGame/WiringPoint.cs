using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WiringPoint : MonoBehaviour
{
    eColor color;



    public eColor GetWiringColor()
    {
        return color;
    }

    public void SetWiringColor(int color)
    {
        this.color = (eColor)color;
    }
}
