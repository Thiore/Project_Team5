using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutSceneFade : MonoBehaviour
{
    public void Fade_In()
    {
        LoadingManager.Instance.FadeIn();
        gameObject.SetActive(false);
    }

    public void Fade_Out()
    {
        LoadingManager.Instance.FadeOut();
    }

    public void SetPlayerPosition()
    { 

        Vector3 setPosition = new Vector3(-7.271f, -2.147f, -8.468f);
        PlayerManager.Instance.mainPlayer.localPosition = setPosition;
        
    }
}
