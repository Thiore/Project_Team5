using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum eBallType
{
    Normal = 0,
    Blue,
    Red,
    Green
}
public class BallObj : MonoBehaviour
{
    [SerializeField] private Transform startValue;
    public Transform getStartValue { get => startValue; }
    [SerializeField] private Transform endValue;
    public WallColor recentPassWall;
    public eBallType ballType;
    private bool isStart;
    private void Awake()
    {
        isStart = false;
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("GameController"))
        {
            Debug.Log("클리어");
        }
    }
    private void OnTriggerExit(Collider col)
    {
        if (col.TryGetComponent(out MazeWall wall))
        {
            if (recentPassWall.Equals(wall.color)&&isStart)
            {
                if (wall.GetExitDirection(this.transform))
                {
                    recentPassWall = wall.color;
                }
                else
                {
                    transform.position = startValue.position;
                    isStart = false;
                }
            }
            else
            {
                isStart = true;
                recentPassWall = wall.color;
            }
        }
    }
}

