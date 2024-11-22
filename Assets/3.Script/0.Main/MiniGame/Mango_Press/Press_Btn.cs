 using System.Collections;
 using System.Collections.Generic;
 using UnityEngine;
 using UnityEngine.UI;

public class Press_Btn : MonoBehaviour
{
    string correctAnswer="RBBGGY";
    string playerAnswer;

    public void BtnClick(Button clickedBtn)
    {
        string value = clickedBtn.GetComponentInChildren<Text>().text;

        playerAnswer= playerAnswer+value;
        Debug.Log($"Player Answer : {playerAnswer}");

        if (playerAnswer.Length >= correctAnswer.Length)
        {
            if (playerAnswer.Equals(correctAnswer))
            {
                Debug.Log("게임 성공");
            }
            else
            {
                Debug.Log("게임 실패");
                playerAnswer = "";
            }
        }
    }
}
