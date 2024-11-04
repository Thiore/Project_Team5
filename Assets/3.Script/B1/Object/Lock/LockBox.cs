using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockBox : MonoBehaviour
{
    private ReadInputData input;
    public GameObject[] cinemachine;
    [SerializeField] GameObject canvas;
    [SerializeField] private LockGame lockGame;

    private void Start()
    {
        TryGetComponent(out input);
        if (input == null)
        {
            Debug.Log("아무것도 없슈");
        }
    }
   

    private void Update()
    {

        TouchBox();
    }

    private void TouchBox()
    {
        if (input.isTouch)
        {
            Debug.Log("상자 디버그");
            cinemachine[0].gameObject.SetActive(true);
            Invoke("CanvasOn", 2f);
            if (lockGame.isanswer)
            {
                CanvasOff();
                cinemachine[1].gameObject.SetActive(true);
                cinemachine[0].gameObject.SetActive(false);
                Debug.Log("dd");
            }
        }
    }

    private void CanvasOn()
    {
        canvas.gameObject.SetActive(true);
    }

    private void CanvasOff()
    {
        canvas.gameObject.SetActive(false);
    }

    


}
