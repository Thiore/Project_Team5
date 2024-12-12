using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pipe : MonoBehaviour
{
    //필수로 지나가야하는 곳 
    [SerializeField] protected bool isimportant;
    public bool Isimportant { get { return isimportant; } }

    [SerializeField] protected Image pipeimage;
    protected bool isImageready;

    public void SetIsImageready()
    {
        isImageready = !isImageready;
    }

    public virtual void PipeImageSet()
    {
        if (isImageready)
        {
            pipeimage.gameObject.SetActive(true);
        }
        else
        {
            pipeimage.gameObject.SetActive(false);
        }
    }
}
