using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_BtnTest : MonoBehaviour
{
    [SerializeField] GameObject target;

    public void TartgetTrue()
    {
        target.SetActive(true);
        gameObject.SetActive(false);
    }

    public void TargetFalse()
    {
        target.SetActive(false);
        gameObject.SetActive(true);
    }
}
