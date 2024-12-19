using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixPipeGameManager : MonoBehaviour
{
    [SerializeField] HashSet<Valve> visitedValves;
    [SerializeField] private Valve startvalve;
    [SerializeField] private Valve endvalve;
    [SerializeField] private GameObject imageobj;
    [SerializeField] private PipeImportantPoint[] pipepoints;
    

    private List<ConnectPipe> connectpipes;

    //제한시간 4분 48초 >> 288초
    private float setTime = 288f;
    private int min;
    private int sec;
   

    private void Awake()
    {
        visitedValves = new HashSet<Valve>();
        connectpipes = new List<ConnectPipe>();
    }

    public void FindPath()
    {
        //다 지우고 다시 그리는 작업 
        InitlizePath();
        FindPath(startvalve);
    }

    // 다음 밸브를 확인하며 연속되게 되는지 확인 

    // 이미지를 그리는건 어쨋든 다음 벨브
    public void FindPath(Valve valve)
    {
        if (valve == null) return;
        if (!visitedValves.Add(valve)) return;
        if (valve.Equals(endvalve)) // && 필수 경로 추가 
        {
            // 최종 종료 조건 
            Debug.Log("마지막 종료");
            int count = 0;            
            foreach(PipeImportantPoint p in pipepoints)
            {
                if (p.CheckisPass()) 
                {
                    count++;
                }                
            }

            if (count.Equals(5))
            {
                Debug.Log("여기서 승리 판정 넣기");
            }
        }

        valve.ShootRay();
        Valve nextvalve = valve.FindNextValve();

        if (valve.DirectionPipe != null)
        {
            if (valve.DirectionPipe.gameObject.TryGetComponent(out ConnectPipe conpipe))
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


    private void InitlizePath()
    {
        visitedValves.Clear();

        for (int i = 0; i < imageobj.transform.childCount; i++)
        {
            if (imageobj.transform.GetChild(i).gameObject.activeSelf)
            {
                imageobj.transform.GetChild(i).gameObject.SetActive(false);
            }
        }

        foreach (PipeImportantPoint p in pipepoints)
        {
            p.InitlizeImage();
        }
    }

    public void ConnectPipeSet(ConnectPipe pipe)
    {
        //연결된 파이프가 2개가 되게끔 유지 
        //파이프가 지워졌다면 
        if (connectpipes.Remove(pipe))
        {
            //지워 진거면 해당 파이프 설정만 바꾸기 메서드 마지막으로 뺌
        }
        else // 파이프가 지워지지 않았다면? 전혀 다른거 들어온거 
        {
            if(connectpipes.Count > 1)
            {
                connectpipes[0].TogglePipeConnection();
                connectpipes.RemoveAt(0);
                connectpipes.Add(pipe);
            }
            else
            {
                connectpipes.Add(pipe);
            }
        }

        pipe.TogglePipeConnection();
        FindPath();
    }

    public IEnumerator StartFixFipeGameTimeLimit()
    {
        yield return null;
    }

}
