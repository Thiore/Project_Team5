using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuseSwithManager : MonoBehaviour
{
    [SerializeField] private Fuse[] emptyfuselist;
    [SerializeField] private Fuse[] resultobjs;

    [SerializeField] private int[] result = new int[] { 0, 1, 1, 2, 2 };


    public void CheckResult()
    {
        int samecount = 0;


        for(int i = 0; i < resultobjs.Length; i++)
        {
            // 비교와 같다면
            if(emptyfuselist[i].GetFuseColor().Equals(result[i]))
            {
                resultobjs[i].SetFuseColor(2);
                samecount++;
            }
            else
            {
                resultobjs[i].SetFuseColor(0);
            }
        }


        if (samecount.Equals(5))
        {
            Debug.Log("성공");
        }
        else
        {
            Debug.Log("다시");          
        }
    }

    public void Reset()
    {
        for(int i = 0; i < emptyfuselist.Length; i++)
        {
            emptyfuselist[i].SetFuseColor(3);
        }
    }
}
