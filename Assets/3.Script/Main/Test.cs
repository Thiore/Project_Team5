using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public void started(Vector2 position)
    {
        Debug.Log("Start");
    }
    public void upd(Vector2 position)
    {
        Debug.Log(position);
    }
    public void end(Vector2 position)
    {
        Debug.Log("end");
    }
}
