using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Keypad_Btn : MonoBehaviour
{
    [SerializeField] private InputField input;
    [SerializeField] private int answer;
    // 선원1실 문 열리기 위한 bool
    public bool isAnswer;

    //정답 이후 원래 player camera로 돌아가기 위한 virtualCam
    [SerializeField] private GameObject virtualCam;

    public void BtnClick(Button clickedBtn)
    {
        string btnName = clickedBtn.name;
        Text btnText = clickedBtn.GetComponentInChildren<Text>();

        if(int.TryParse(btnText.text,out int result))
        {
            if (input.text.Length >= 3)
            {
                Debug.Log("Too many input");
                return;
            }
            input.text += btnText.text;
        }
        else
        {
            if (btnText.text.Equals("Enter"))
            {
                Debug.Log($"Answer is {input.text}");
                if (input.text.Equals(answer.ToString()))
                {
                    //정답 이후 시네머신 off
                    virtualCam.gameObject.SetActive(false);
                }
                input.text = "";
            }
            else
            {
                input.text=input.text.Substring(0, input.text.Length - 1);
            }
        }
    }
}
