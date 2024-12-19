using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PipeImportantPoint : MonoBehaviour
{
    [SerializeField] private Sprite[] ipimagesprites;
    private Image ipimage;
    private bool ispass;

    private void Awake()
    {
        TryGetComponent(out ipimage);
    }
    
    // 안지나갔으면 0 / 지나갔으면 1
    public void SetIPImage(bool setactive)
    {
        if (setactive)
        {
            ipimage.sprite = ipimagesprites[1];  
            ispass = true;
        }
        else
        {
            ipimage.sprite = ipimagesprites[0];   
            ispass = false;
        }
    }

    public void InitlizeImage()
    {
        ipimage.sprite = ipimagesprites[0];
        ispass = false;
    }

    public bool CheckisPass()
    {
        return ispass;
    }

}
