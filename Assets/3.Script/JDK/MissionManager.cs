using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionManager : MonoBehaviour
{
    [SerializeField] private GameObject[] mission = null;

    public void Mission(int index)
    {
        for (int i = 0; i < mission[index].transform.childCount; i++)
        {
            mission[index].transform.GetChild(i).gameObject.SetActive(true);
        }
    }

    public void Close(int index)
    {
        for (int i = 0; i < mission[index].transform.childCount; i++)
        {
            mission[index].transform.GetChild(i).gameObject.SetActive(false);
        }
    }
}