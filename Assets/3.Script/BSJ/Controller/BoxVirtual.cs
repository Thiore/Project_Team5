using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BoxVirtual : MonoBehaviour
{
    private ReadInputData input;
    public LockGame lockgame;
    private bool istouching;
    [SerializeField] private Animator top;
    [SerializeField] private GameObject cam;
    [SerializeField] private GameObject cam2;
    public GameObject canvas;

    private void Start()
    {
        
        TryGetComponent(out input);
    }

    private void Update()
    {
        if (input.isTouch)
        {
            if (!lockgame.isAnswer)
            { 
            cam.gameObject.SetActive(true);
                canvas.gameObject.SetActive(true);


                if (lockgame.isAnswer)
                    {
                        cam.gameObject.SetActive(false);
                        cam2.gameObject.SetActive(true);
                    istouching = true;
                    top.SetBool("isOpen", true);
                    Invoke("test", 3f);
                    if (istouching && cam2.activeSelf && input.isTouch)
                    {
                        cam2.gameObject.SetActive(false);
                        istouching = false;
                        top.SetBool("isOpen", false);
                    }

                    }
            }
            else if (lockgame.isAnswer)
            {
                cam.gameObject.SetActive(false);
                cam2.gameObject.SetActive(true);
                top.SetBool("isOpen", true);

                if (istouching && cam2.activeSelf && input.isTouch)
                {
                    cam2.gameObject.SetActive(false);
                    istouching = false;
                    top.SetBool("isOpen", false);
                }
            }
        }
    }

    public void test()
    {
        GameManager.Instance.LoadSlide();
    }
}
