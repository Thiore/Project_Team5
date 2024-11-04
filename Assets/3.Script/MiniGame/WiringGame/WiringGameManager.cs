using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//컬러 순서로 전선 값 배치했음 수정할꺼면 말해주세요 
public enum eColor
{
   Green = 0,
   Orange,
   Red,
   Yellow
}

public class WiringGameManager : MonoBehaviour
{
    [SerializeField] private GameObject[] startPoints;
    [SerializeField] private GameObject[] endPoints;
    [SerializeField] private GameObject[] wirings;

    private WiringPoint[] endWiringPoints;

    private void Start()
    {
        InitGame();
    }

    public void InitGame()
    {
        InitPoints();

    }


    //배선 시작 끝 위치 섞기
    private void InitPoints()
    {
        endWiringPoints = new WiringPoint[4];

        //각 시작지점/끝지점/배전 색상 및 boolean 초기화 
        for (int i = 0; i < startPoints.Length; i++) 
        {
            if(startPoints[i].TryGetComponent(out WiringPoint startwiringpoint))
            {
                startwiringpoint.SetWiringColor(i);
                startwiringpoint.SetboolConnect(true);
            }

            if (endPoints[i].TryGetComponent(out WiringPoint endwiringpoint))
            {
                endwiringpoint.SetWiringColor(i);
                endwiringpoint.SetboolConnect(false);
                endWiringPoints[i] = endwiringpoint;
                
            }

            if (wirings[i].TryGetComponent(out Wiring wiring))
            {
                wiring.SetWiringColor(i);
            }
        }

        int shufflecount = Random.Range(1, 10);
        for(int i = 0; i < shufflecount; i++)
        {
            int index = Random.Range(0, 4);
            int nextindex = Random.Range(0, 4);

            if(index == nextindex)
            {
                nextindex = Random.Range(0, 4);
            }

            Vector3 emptypos = startPoints[index].transform.position;
            startPoints[index].transform.position = startPoints[nextindex].transform.position;
            startPoints[nextindex].transform.position = emptypos;
        }

        for (int i = 0; i < shufflecount; i++)
        {
            int index = Random.Range(0, 4);
            int nextindex = Random.Range(0, 4);

            if (index == nextindex)
            {
                nextindex = Random.Range(0, 4);
            }

            Vector3 emptypos = endPoints[index].transform.position;
            endPoints[index].transform.position = endPoints[nextindex].transform.position;
            endPoints[nextindex].transform.position = emptypos;
        }

        // 다 섞고 배선 위치 정렬
        for (int i = 0; i < startPoints.Length; i++)
        {
            Vector2 wiringspos = new Vector2(startPoints[i].transform.localPosition.x + 66f, startPoints[i].transform.localPosition.y);
            wirings[i].transform.position = wiringspos;
        }


    }


    private void CheckWiringBool()
    {
        int check = 0;
        for(int i = 0; i < endWiringPoints.Length; i++)
        {
            if (endWiringPoints[i].IsConncet)
            {
                check++;
            }
        }

        if(check >= 4)
        {
            // 이거 게임 끝 
        }
        else
        {
            check = 0;
        }
    }
}

