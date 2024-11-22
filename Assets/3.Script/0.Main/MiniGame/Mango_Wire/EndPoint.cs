 using System.Collections;
 using System.Collections.Generic;
 using UnityEngine;

public class EndPoint : MonoBehaviour,ISetColor
{
    public WireColor color;
    public bool isconnected;
    public Transform lineEndPostion;

    public Material redMat;
    public Material yellowMat;
    public Material greenMat;
    public Material orangeMat;
    [SerializeField]private Renderer render;

    public void setColor(WireColor color)
    {
        this.color = color;
    }

    public void setMaterial()
    {
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
