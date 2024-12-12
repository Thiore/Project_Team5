using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForkLiftAnswer : MonoBehaviour
{
    [SerializeField] private List<ForkLiftCollect> allCollectZones; //모든 정답 구역
    private void Start()
    {
        //정답 상태 초기화
        foreach(var zone in allCollectZones)
        {
            zone.isCollect = false;
        }
    }

    public void CheckAllZones()
    {
        foreach (var zone in allCollectZones)
        {
            //하나라도 정답이 아니라면 종료
            if (!zone.isCollect)
            {
                Debug.Log("아직 모든 구역이 정답 상태가 아님");
                return;
            }
        }
        //모든 구역이 정답 상태일 때
        Debug.Log("Finish All"); 
    }
}
