using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Voltmeter : MonoBehaviour
{
    [SerializeField] private GameObject[] cylinderArray = new GameObject[2];
    [SerializeField] private GameObject[] btnArray = new GameObject[4];
    private VoltBtn[] btnComponentArray = new VoltBtn[4];
    private ReadInputData[] inputArray=new ReadInputData[4];
    public float correctAnswer;

    private bool isRotating;

    private void Start()
    {
        for(int i=0; i < 4; i++)
        {
            inputArray[i] = btnArray[i].GetComponent<ReadInputData>();
            btnComponentArray[i] = btnArray[i].GetComponent<VoltBtn>();
        }

    }
    private void Update()
    {
        foreach(var input in inputArray)
        {
            if (input.isTouch&&!isRotating)
            {
                int index = System.Array.IndexOf(inputArray, input);
                btnComponentArray[index].BtnClick();
                isRotating = true;

            }
        }
    }

    private void RotateStateChange()
    {
        isRotating = false;
    }
    
}
