using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public enum ConnectionColor
{
    Red,
    Blue,
    Gray,
    Pink,
    Orange,
    Yellow,
    Sky_Blue,
}

public class Mission : MonoBehaviour
{
    [Header("Line ¾îÅÂÄ¡")]
    [SerializeField] private LineRenderer lr = null;

    // [SerializeField] private ConnectionColor color;
    public void ChangeColor(Material color)
    {
        lr.material = color;
    }
}