using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Book : MonoBehaviour, ITouchable
{
    [SerializeField] private Animator anim;
    [SerializeField] private int page;
    private bool isBack;

    private void Awake()
    {
        isBack = false;
    }
    public void OnTouchEnd(Vector2 position)
    {
        isBack = !isBack;
        switch (page)
        {
            case 1:
                anim.SetBool("isFirst", isBack);
                break;
            case 2:
                anim.SetBool("isSecond", isBack);
                break;
            case 3:
                break;
        }
    }

    public void OnTouchHold(Vector2 position)
    {
       
    }

    public void OnTouchStarted(Vector2 position)
    {
        
    }
}
