using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fuse : MonoBehaviour
{
    private enum eFuseColor
    {
        Red = 0,
        Blue,
        Green, 
        White
    }


    [SerializeField] private eFuseColor color;
    [SerializeField] private Material[] colors;
    private MeshRenderer mesh;

    private void OnEnable()
    {
        TryGetComponent(out mesh);
    }

    public void SetFuseColor(int num)
    {
        color = (eFuseColor)num;
        mesh.material = colors[num];
    }

    public int GetFuseColor()
    {
        //Debug.Log((int)color);
      
        return (int)color;
    }

}
