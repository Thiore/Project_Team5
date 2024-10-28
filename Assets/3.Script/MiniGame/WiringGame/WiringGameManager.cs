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
        for (int i = 0; i < startPoints.Length; i++) 
        {
            startPoints[i].GetComponent<WiringPoint>().SetWiringColor(i);
            startPoints[i].GetComponent<WiringPoint>().IsConncet = true;
            endPoints[i].GetComponent<WiringPoint>().SetWiringColor(i);
            endPoints[i].GetComponent<WiringPoint>().IsConncet = false;
            wirings[i].GetComponent<Wiring>().SetWiringColor(i);
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
            Vector2 wiringspos = new Vector2(startPoints[i].transform.position.x + 66f, startPoints[i].transform.position.y);
            wirings[i].transform.position = wiringspos;
        }


    }


    private void CheckWiring()
    {
        for(int i = 0; i < wirings.Length; i++)
        {
            
        }
    }
}

