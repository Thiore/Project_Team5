using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyTestMM : MonoBehaviour
{
    private ReadInputData input;
    private GameObject playInterface;
    private GameObject keypadUI;



    private void Start()
    {
        TryGetComponent(out input);
        FindObjectUI();
    }

    private void Update()
    {
        if (input.isTouch)
        {
            KeyStart();
        }
    }
    private void FindObjectUI()
    { 
    if(gameObject != null)
        {
            Transform a = gameObject.transform.GetChild(0);
            keypadUI = a.gameObject;

        }
    }

    public void KeyStart()
    {
        playInterface.gameObject.SetActive(false);
        keypadUI.gameObject.SetActive(true);
    }

    public void KeyEnd()
    {
        playInterface.gameObject.SetActive(true);
        keypadUI.gameObject.SetActive(false);
    }
    private void OnEnable()
    {
        playInterface = GameObject.FindGameObjectWithTag("PlayInterface");
    }
}
