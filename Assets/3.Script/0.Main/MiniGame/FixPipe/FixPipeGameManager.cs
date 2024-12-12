using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FixPipeGameManager : MonoBehaviour
{
    [SerializeField] HashSet<Valve> visitedValves;
    [SerializeField] private Valve startvalve;
    [SerializeField] private Valve endvalve;

    private List<Pipe> pipes;

    private void Awake()
    {
        visitedValves = new HashSet<Valve>();
        pipes = new List<Pipe>();

    }

    public void FindPath()
    {
        visitedValves.Clear();
        pipes.Clear();
        FindPath(startvalve);
    }
    // 처음부터 벨브를 지나가며 확인

    public void FindPath(Valve valve)
    {
        if (valve == null) return; // 초기 조건 확인

        // 이미 방문한거면 XXX
        if (!visitedValves.Add(valve)) return;

        // 현재 Vale 에 따른 Pipe 등록
        if (valve.DirectionPipe != null)
        {
            pipes.Add(valve.DirectionPipe);
        }

        Valve nextvalve = valve.FindNextValve();
        if (nextvalve == null)
        {
            if(pipes.Count > 0)
            {
                PipesDrawAndClear();
                return;
            }
        }
        else
        {
            //파이프 리무브도 생각
            pipes.Add(valve.DirectionPipe);
        }
   

        FindPath(nextvalve);
    }


    private void PipesDrawAndClear()
    {
        foreach (var pipe in pipes)
        {
            pipe.PipeImageSet();
        }
        pipes.Clear();
    }
}
