using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class UseClue : MonoBehaviour, IUITouchable
{
    private Coroutine clue_co;
    private bool isOpen;
    private float scaleTime;

    private GameObject currentObj;

    private void OnEnable()
    {
        clue_co = null;
        transform.localScale = Vector3.one;
        isOpen = false;
        scaleTime = 0;
    }
    private void OnDisable()
    {
        if (clue_co != null)
        {
            StopCoroutine(clue_co);
            clue_co = null;
        }
        transform.localScale = Vector3.one;
        isOpen = false;
    }

    public void OnUIEnd(PointerEventData data)
    {
    }

    public void OnUIHold(PointerEventData data)
    {
    }

    public void OnUIStarted(PointerEventData data)
    {
        isOpen = !isOpen;
        if (clue_co == null)
            clue_co = StartCoroutine(Open_co());
    }

    private IEnumerator Open_co()
    {
        while(true)
        {
            
            if(isOpen)
            {
                scaleTime += Time.deltaTime*2f;
                scaleTime = Mathf.Clamp(scaleTime, 0f, 1f);
                transform.localScale = Vector3.one * (1f + scaleTime * 3f);
                yield return null;
                if (scaleTime>=1f)
                {
                    clue_co = null;
                    yield break;
                }
                    
            }
            else
            {
                scaleTime -= Time.deltaTime*2f;
                scaleTime = Mathf.Clamp(scaleTime, 0f, 1f);
                transform.localScale = Vector3.one * (1f + scaleTime * 3f);
                yield return null;
                if (scaleTime<=0f)
                {
                    clue_co = null;
                    yield break;
                }
            }
            
        }
        
    }

}
