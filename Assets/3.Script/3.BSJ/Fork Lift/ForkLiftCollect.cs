using System.Collections.Generic;
using UnityEngine;

public class ForkLiftCollect : MonoBehaviour
{
    [SerializeField] private int[] myCollect; // 각 구역 정답 인덱스
    public bool isCollect = false; // 각 구역의 정답 여부
    private ForkLiftAnswer answer; //정답 관리자


    private void Start()
    {
        answer = gameObject.GetComponentInParent<ForkLiftAnswer>();
    }

    // 전달받은 인덱스들과 정답 비교
    public void CheckCompletion(List<int> detectedIndices)
    {
        if (detectedIndices.Count != myCollect.Length)
        {
            isCollect = false;
            //return;
        }
        else
        {

            foreach (int index in myCollect)
            {
                if (!detectedIndices.Contains(index))
                {
                    isCollect = false;
                    return;
                }
            }

        isCollect = true;
        Debug.Log("Finish");
        }

        //모든 정답 관리자에게 전달
        answer.CheckAllZones();

    }
}
