using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Keypad_Btn : MonoBehaviour
{
    [SerializeField] private InputField input;
    [SerializeField] private int answer;

    //상호작용 관련
    [SerializeField] private int floorIndex; //오브젝트의 현재 층
    [SerializeField] private int objectIndex; //오브젝트 본인의 인덱스
    private SaveManager saveManager; //상태관리

    private void Start()
    {
        saveManager = GameObject.FindGameObjectWithTag("SaveManager").GetComponent<SaveManager>();
    }

    public void BtnClick(Button clickedBtn)
    {
        string btnName = clickedBtn.name;
        Text btnText = clickedBtn.GetComponentInChildren<Text>();

        if (int.TryParse(btnText.text, out int result))
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
                    saveManager.UpdateObjectState(floorIndex, objectIndex, true);
                }
                input.text = "";
            }
            else
            {
                input.text = input.text.Substring(0, input.text.Length - 1);
            }
        }
    }
}
