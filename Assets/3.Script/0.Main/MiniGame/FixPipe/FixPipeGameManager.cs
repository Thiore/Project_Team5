using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FixPipeGameManager : MonoBehaviour
{
    [SerializeField] private TMP_Text tobecontinue;

    [SerializeField] HashSet<Valve> visitedValves;
    [SerializeField] private Valve startvalve;
    [SerializeField] private Valve endvalve;
    [SerializeField] private GameObject imageobj;
    [SerializeField] private PipeImportantPoint[] pipepoints;

    [SerializeField] private TMP_Text limittimeText;
    private bool iscomplete;
    private List<ConnectPipe> connectpipes;

    

    //제한시간 4분 48초 >> 288초
    private float limittime = 300f;
    private int min;
    private float sec;

    [SerializeField] private GameObject CargoRoomCam;
    [SerializeField] private GameObject MonitorCam;

    [SerializeField] private int floorIndex;
    [SerializeField] private int objectIndex;
    public int getObjectIndex { get => objectIndex; }

    [SerializeField] private Animator interactionAnim;



    private void Awake()
    {
        if (DataSaveManager.Instance.GetGameState(floorIndex, objectIndex))
        {
            iscomplete = true;
            interactionAnim.SetBool("Open", true);
        }
        else
        {
            iscomplete = false;
        }
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
            int count = 0;
            foreach (PipeImportantPoint p in pipepoints)
            {
                if (p.CheckisPass())
                {
                    count++;
                }
            }

            if (count.Equals(5))
            {
                iscomplete = true;
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
        //파이프가 지워졌다면 파이프 설정만 바꾸기
        if (!connectpipes.Remove(pipe))
        {// 파이프가 지워지지 않았다면? 다른 파이프입니다.
            if (connectpipes.Count < 2)
            {
                connectpipes.Add(pipe);
            }
            else
            {
                connectpipes[0].TogglePipeConnection();
                connectpipes.RemoveAt(0);
                connectpipes.Add(pipe);
            }
        }

        pipe.TogglePipeConnection();
        FindPath();
    }
    public void GameStart()
    {
        StartCoroutine(StartFixFipeGameTimeLimit_co());
    }

    private IEnumerator StartFixFipeGameTimeLimit_co()
    {

        while(limittime > 0)
        {
            if (iscomplete)
            {
                DataSaveManager.Instance.UpdateGameState(floorIndex,objectIndex);


                MonitorCamOn();
                yield break;
            }

            limittime -= 1;
            min = (int)limittime / 60;
            sec = Mathf.FloorToInt(limittime % 60f);
            limittimeText.text = $" {min:00} : {sec:00}";

            yield return new WaitForSeconds(1f);
        }

        GameManager.Instance.LoadLobby();
        yield break;
        

    }

    private void MonitorCamOn()
    {
        MonitorCam.SetActive(true);
        Invoke("CargoRoomCamOn", 6f);
    }
    private void CargoRoomCamOn()
    {
        PlayerManager.Instance.resetCam.SetActive(true);
        CargoRoomCam.SetActive(true);
        MonitorCam.SetActive(false);
        Invoke("CargoRoomCamOff", 2f);
    }
    private void CargoRoomCamOff()
    {
        interactionAnim.SetBool("Open", true);

        //tobecontinue.gameObject.SetActive(true);
        //string x = "To Be NextWeek......";
        //StartCoroutine(DialogueManager.Instance.ReavealText(tobecontinue, x));
        Invoke("ResetCamera", 4f);
    }
    private void ResetCamera()
    {
        CargoRoomCam.SetActive(false);
        PlayerManager.Instance.ResetCamOff();
    }
}
