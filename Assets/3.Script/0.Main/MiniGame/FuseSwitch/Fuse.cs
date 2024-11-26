using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fuse : MonoBehaviour
{
    private enum eFuseColor
    {
        Red = 0,
        Green, 
        Blue
    }

    [SerializeField] private eFuseColor color;
    public void SetFuseColor(int num)
    {
        color = (eFuseColor)num;
    }

}
