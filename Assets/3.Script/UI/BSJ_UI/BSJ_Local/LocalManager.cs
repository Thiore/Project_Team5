using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings;
public class LocalManager : MonoBehaviour
{
    private bool isChanging;
    public DialogueManager dia;
    public void ChangeLocale(int index)
    {
        if (isChanging)
            return;

        StartCoroutine(ChangeRoutine_co(index));
    }

    private IEnumerator ChangeRoutine_co(int index)
    {
        isChanging = true;

        yield return LocalizationSettings.InitializationOperation; //초기화

        // 언어 바꿔주기 SelectedLocale에 있는 언어로
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[index];

        isChanging = false;

        dia.SetDialogue("B1", 2);
    }
}
