using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FixPipeGameManager : MonoBehaviour
{
    [SerializeField] HashSet<Valve> visitedValves;
    [SerializeField] private Valve startvalve;
    [SerializeField] private Valve endvalve;
    [SerializeField] private GameObject imageobj;

    private List<Pipe> pipes;

    private void Awake()
    {
        visitedValves = new HashSet<Valve>();
        pipes = new List<Pipe>();
    }

    public void FindPath()
    {
        //다 지우고 다시 그리는 작업 
        visitedValves.Clear();
        pipes.Clear();
        for (int i = 0; i < imageobj.transform.childCount; i++)
        {
            if (imageobj.transform.GetChild(i).gameObject.activeSelf)
            {
                imageobj.transform.GetChild(i).gameObject.SetActive(false);
            }
        }

        FindPath(startvalve);
    }

    // 다음 밸브를 확인하며 연속되게 되는지 확인 

    // 이미지를 그리는건 어쨋든 다음 벨브
    public void FindPath(Valve valve)
    {
        if (valve.Equals(endvalve)) // && 필수 경로 추가 
        {
            // 최종 종료 조건 
        }
        if (!visitedValves.Add(valve)) return;
        if (valve == null) return;

        valve.ShootRay();
        Valve nextvalve = valve.FindNextValve();
        
        if (valve.DirectionPipe != null)
        {
            if(valve.DirectionPipe.gameObject.TryGetComponent(out ConnectPipe conpipe))
            {
                if (!conpipe.IsConnect)
                {
                    conpipe.ShotPipeImageSet();
                    return;
                }
            }
            valve.DirectionPipe.PipeImageSet();
        }

        Debug.Log("호출");
        FindPath(nextvalve);
    }


    private void PipesDraw()
    {
        if (pipes.Count > 0)
        {
            foreach (var pipe in pipes)
            {
                pipe.PipeImageSet();
            }
        }
    }
}
