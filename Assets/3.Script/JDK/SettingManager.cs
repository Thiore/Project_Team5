using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SettingManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI KoreaLanguageText = null;
    [SerializeField] private TextMeshProUGUI EnglishLanguageText = null;

    [Header("언어 선택 색 프로퍼티")]
    [SerializeField] private Color LanguageChoiceColor;

    public void Language(string sentence)
    {
        switch (sentence)
        {
            case "Korea":

                KoreaLanguageText.color = LanguageChoiceColor;
                EnglishLanguageText.color = Color.white;

                break;

            case "English":

                KoreaLanguageText.color = Color.white;
                EnglishLanguageText.color = LanguageChoiceColor;

                break;

            default:

                Debug.Log("설정 언어 선택 인덱스 오류");

                break;
        }
    }
}