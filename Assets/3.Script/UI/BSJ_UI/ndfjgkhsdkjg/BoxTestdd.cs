using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxTestdd : MonoBehaviour
{
    public LockGame lockgame;
    private Animator ani;
    public GameObject camera;

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
        }
    }

    private void sdjf()
    {
        camera.gameObject.SetActive(false);
    }
}
