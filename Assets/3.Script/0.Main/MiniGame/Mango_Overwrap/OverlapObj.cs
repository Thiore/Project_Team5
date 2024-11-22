using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverlapObj : MonoBehaviour
{
    public LineRenderer line;
    public float lineWidth;
    public bool isConnected;
    public bool isPoint;
    public OverlapColor connectColor;
    public Vector3 linePos;
    private void Awake()
    {
        line = GetComponent<LineRenderer>();

        line.startWidth = lineWidth;
        line.endWidth = lineWidth;
        line.enabled = false;

        linePos = transform.TransformPoint(new Vector3(0,0.5f,0));

        if (this.GetComponent<OverlapPoint>() != null)
        {
            this.connectColor = GetComponent<OverlapPoint>().startColor;
            isPoint = true;
        }
        else
        {
            connectColor = OverlapColor.Null;
        }
            
            
    }
}
