using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookPage : MonoBehaviour, ITouchable
{
    [SerializeField] private Animator anim;
    [SerializeField] private int page;
    private bool isBack;

    private void Awake()
    {
        isBack = false;
    }
    private void OnEnable()
    {
        SettingPage();
    }
    public void OnTouchEnd(Vector2 position)
    {
        isBack = !isBack;
        SettingPage();
    }

    private void SettingPage()
    {
        switch (page)
        {
            case 1:
                anim.SetBool("isFirst", isBack);
                break;
            case 2:
                anim.SetBool("isSecond", isBack);
                break;
            case 3:
                anim.SetBool("isThird", isBack);
                break;
            case 4:
                anim.SetBool("isFourth", isBack);
                break;
            case 5:
                anim.SetBool("isFifth", isBack);
                break;
            case 6:
                anim.SetBool("isSixth", isBack);
                break;
            case 7:
                anim.SetBool("isSeventh", isBack);
                break;
            case 8:
                anim.SetBool("isEight", isBack);
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
