using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UseButton : MonoBehaviour
{
    [SerializeField] private Toggle quickBtn;
    private Animator quickAnim;
    private readonly int openAnim = Animator.StringToHash("Open");

    private void Start()
    {
        quickBtn.TryGetComponent(out quickAnim);
        quickAnim.SetBool(openAnim, !quickBtn.isOn);
    }
    public void QucikSlotButton(bool isOn)
    {
        if(!quickBtn.isOn.Equals(isOn))
        {
            quickAnim.SetBool(openAnim, quickBtn.isOn);
        }
        else
        {
            quickAnim.SetBool(openAnim, isOn);
        }
        
    }
}
