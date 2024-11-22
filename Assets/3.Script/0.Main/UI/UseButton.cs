using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UseButton : MonoBehaviour
{
    [SerializeField] private Toggle quickBtn;
    private Animator quickAnim;
    private readonly int openAnim = Animator.StringToHash("Open");

    private void OnEnable()
    {
        quickBtn.TryGetComponent(out quickAnim);
        quickAnim.SetBool(openAnim, quickBtn.isOn);
    }
    public void QucikSlotButton(bool isOn)
    {
        
        quickBtn.isOn = isOn;
        
        quickAnim.SetBool(openAnim, quickBtn.isOn);
        
    }

    public void QucikSlotEnable()
    {
        quickAnim.SetBool(openAnim, quickBtn.isOn);
    }
}
