 using System.Collections;
 using System.Collections.Generic;
 using UnityEngine;
 using TMPro;

public class StoryBox_Loading : MonoBehaviour
{
    public TMP_Text loadingText; // "..." 반복될 Text
    private Coroutine loading_Co;

    private void OnEnable()
    {
        if (loading_Co == null)
        {
            loading_Co = StartCoroutine(UpdateLoadingText_co());
        }
    }

    private void OnDisable()
    {
        if (loading_Co != null)
        {
            StopCoroutine(loading_Co);
            loading_Co = null;
        }

        if (loadingText != null)
        {
            loadingText.text = ""; //텍스트 초기화
        }
    }

    private IEnumerator UpdateLoadingText_co()
    {
        string[] dots = { ".", "..", "..." };
        int index = 0;
        float addTime = 0f; //시간 추가
        float duration = 2f; //2초 동안 반복

        while (addTime < duration)
        {
            loadingText.text = dots[index]; //Text 업데이트
            index = (index + 1) % dots.Length; //인덱스 순환
            addTime += 0.5f;
            yield return new WaitForSeconds(0.5f); //2초 주기를 위한 0.5초 대기
        }

        //3초 후 "..." 상태로 고정
        loadingText.text = "...";
        loading_Co = null;
    }
}
