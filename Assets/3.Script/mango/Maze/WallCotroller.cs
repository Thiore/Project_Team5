using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallCotroller : MonoBehaviour
{
    [SerializeField] private eBallType ballType;
    [SerializeField] private BallController ball;

    [SerializeField] private WallCotroller[] walls;

    public BoxCollider[] col { get; private set; }
    

    private void Start()
    {
        col = new BoxCollider[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
            transform.GetChild(i).TryGetComponent(out col[i]);
    }

    public void IsDisableTrigger()
    {
        if(ball.ballType.Equals(ballType))
        {
            for(int i = 0; i < col.Length;i++)
            {
                col[i].isTrigger = false;
            }
        }
        for (int i = 0; i < walls.Length; i++)
        {
            IsEnableTrigger();
        }
    }

    

    public void IsEnableTrigger()
    {
        for (int i = 0; i < col.Length; i++)
        {
            col[i].isTrigger = true;
        }
    }
}
