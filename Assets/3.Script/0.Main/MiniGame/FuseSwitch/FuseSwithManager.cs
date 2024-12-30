using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FuseSwithManager : MonoBehaviour
{
    [SerializeField] private Fuse[] resultfuselist;
    [SerializeField] private Fuse_Binker[] blinkers;
    [SerializeField] private Image[] fuseimages;

    private int[] result = new int[] { 0, 2, 1, 0, 1, 2 };

    private bool ischeck;

    public void CheckResult()
    {
        //Debug.Log(resultfuselist.Length);
        //Debug.Log(result.Length);
        int samecount = 0;


        for (int i = 0; i < resultfuselist.Length; i++)
        {
            //비었을때 누른다면
            if (i < 4 && !resultfuselist[i].gameObject.activeSelf)
            {
                blinkers[i].SetBlinkerColor(0);
            }

            // 비교와 같다면
            if (i < 4 && resultfuselist[i].gameObject.activeSelf)
            {
                if (resultfuselist[i].GetFuseColor().Equals(result[i]))
                {
                    blinkers[i].SetBlinkerColor(1);
                    samecount++;
                }
                else
                {
                    blinkers[i].SetBlinkerColor(0);
                }
            }
        }

        if (resultfuselist[4].gameObject.activeSelf && resultfuselist[5].gameObject.activeSelf)
        {
            if (resultfuselist[4].GetFuseColor().Equals(result[4]) && resultfuselist[5].GetFuseColor().Equals(result[5]))
            {
                blinkers[4].SetBlinkerColor(1);
                samecount++;
            }
            else
            {
                blinkers[4].SetBlinkerColor(0);
            }
        }
        else
        {
            blinkers[4].SetBlinkerColor(0);
        }


        if (samecount.Equals(5))
        {
            //Debug.Log("성공");
        }
        else
        {
            //Debug.Log("다시");
            FuseReset();
        }
    }

    public void FuseReset()
    {
        
        for (int i = 0; i < resultfuselist.Length; i++)
        {
            if (resultfuselist[i].gameObject.activeSelf)
            {
                resultfuselist[i].SetFuseColor(3);
                resultfuselist[i].gameObject.SetActive(false);
            }
        }

        for (int i = 0; i < fuseimages.Length; i++) 
        {
            if (fuseimages[i].enabled.Equals(false))
            {
                fuseimages[i].enabled = true;
            }
        }
    }
}
