using System.Collections.Generic;
using UnityEngine;

public class ForkLiftCollect : MonoBehaviour
{
    [SerializeField] private int[] myCollect; // 각 구역 정답 인덱스
    public bool isCollect = false; // 각 구역의 정답 여부
    [SerializeField] private InteractionForkLift answer; //정답 관리자



    // 전달받은 인덱스들과 정답 비교
    public void CheckCompletion(List<int> detectedIndices, int lastIndex)
    {
        //1. 길이 체크 : 감지된 인덱스 수와 정답 인덱스 수가 다르면 오답
        if (detectedIndices.Count != myCollect.Length)
        {
            isCollect = false;
            //Debug.Log("개수 맞지 않음");
            answer.CheckAllZones(); //실패 전달
            return;
        }

        //2. 정렬 순서 비교
        for (int i = 0; i < myCollect.Length; i++)
        {
            if (detectedIndices[i] != myCollect[i])
            {
                isCollect = false;
                //Debug.LogWarning($"인덱스 순서가 맞지 않습니다. 기대값: {myCollect[i]}, 실제값: {detectedIndices[i]}");
                answer.CheckAllZones(); //실패 전달
                return;
            }
        }

        //3. 마지막 인덱스 확인
        if (myCollect[myCollect.Length - 1] != lastIndex)
        {
            isCollect = false;
            //Debug.LogWarning($"마지막 인덱스가 맞지 않습니다. 기대값: {myCollect[myCollect.Length - 1]}, 실제값: {lastIndex}");
        answer.CheckAllZones(); //실패 전달
        return;
        }

        //4. 모든 조건 충족 시
        isCollect = true;
        //Debug.Log("Finish: 모든 조건을 충족했습니다.");
        
        // 모든 정답 관리자에게 성공을 전달
        answer.CheckAllZones();


        //if (detectedIndices.Count != myCollect.Length)
        //{
        //    isCollect = false;
        //    //return;
        //}
        //else
        //{

        //    foreach (int index in myCollect)
        //    {
        //        if (!detectedIndices.Contains(index))
        //        {
        //            isCollect = false;
        //            return;
        //        }
        //    }

        //isCollect = true;
        //Debug.Log("Finish");
        //}

        ////모든 정답 관리자에게 전달
        //answer.CheckAllZones();

    }
}
