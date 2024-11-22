 using System.Collections;
 using System.Collections.Generic;
 using UnityEngine;

public class SlideBtn : MonoBehaviour
{
    public List<GameObject> objList;
    public void ObjPosReset()
    {
        foreach(var obj in objList)
        {
            obj.GetComponent<SlideObject>().InitPosition();
        }
    }
}
