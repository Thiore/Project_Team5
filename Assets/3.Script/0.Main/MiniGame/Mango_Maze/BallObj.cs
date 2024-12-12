using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallObj : MonoBehaviour
{
    public WallColor recentPassWall;
    public Vector3 startPos;
    private bool isStart;
    private void Awake()
    {
        isStart = false;
    }

    private void OnTriggerExit(Collider wall)
    {
        if (wall.GetComponent<MazeWall>() != null)
        {
            if (recentPassWall.Equals(wall.GetComponent<MazeWall>().color)&&isStart)
            {
                if (wall.GetComponent<MazeWall>().GetExitDirection(this.transform))
                {
                    recentPassWall = wall.GetComponent<MazeWall>().color;
                }
                else
                {
                    transform.position = startPos;
                    isStart = false;
                }
            }
            else
            {
                isStart = true;
                recentPassWall = wall.GetComponent<MazeWall>().color;
            }
        }
    }
}

public enum WallColor
{
    Blue,
    Red,
    Green
}

