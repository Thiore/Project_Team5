using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxTestdd : MonoBehaviour
{
    public LockGame lockgame;
    private Animator ani;
    public GameObject cam;

    private void Start()
    {
        
        ani = GetComponent<Animator>();
    }

    private void Update()
    {
        if (lockgame.isanswer)
        {
            ani.SetTrigger("Open");
            Invoke("sdjf", 3f);
            Invoke("LoadSlide",5f);
        }
    }
    private void LoadSlide()
    {
        GameManager.Instance.LoadSlide();
    }
    private void sdjf()
    {
        cam.gameObject.SetActive(false);
    }
}
