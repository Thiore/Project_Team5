 using System.Collections;
 using System.Collections.Generic;
 using UnityEngine;
 using UnityEngine.UI;
public class Pin : MonoBehaviour
{
    private Toggle pinToggle;
    private Animator anim;
    private void Awake()
    {
        TryGetComponent(out pinToggle);
        TryGetComponent(out anim);
    }
    private void OnEnable()
    {
        if(pinToggle.isOn)
        {
            pinToggle.isOn = false;
            AnimatorStateInfo animState =  anim.GetCurrentAnimatorStateInfo(0);
            if (!animState.IsName("Empty"))
            {
                anim.Play("Empty");
            }
        }
    }

    public void ToggleIsOn()
    {
        anim.SetBool("isOn", pinToggle.isOn);
    }
}
