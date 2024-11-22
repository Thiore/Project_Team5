 using System.Collections;
 using System.Collections.Generic;
 using UnityEngine;
 using System;

public class ShuffleColor : MonoBehaviour
{
    public StartPoint[] startArray;
    public EndPoint[] endArray;
    private WireColor[] colors = { WireColor.Red, WireColor.Orange, WireColor.Green, WireColor.Yellow };
    private void OnEnable()
    {
        ShuffleArray(startArray);
        ShuffleArray(endArray);
        AssignRandomColors(startArray,endArray);
    }

    private void ShuffleArray<T>(T[] array)
    {
        for (int i = array.Length - 1; i > 0; i--)
        {
            int randomIndex = UnityEngine.Random.Range(0, i + 1); // 현재 인덱스까지의 무작위 인덱스를 선택
            T temp = array[i];
            array[i] = array[randomIndex];
            array[randomIndex] = temp;
        }
    }

    // 배열의 각 요소에 무작위 색상 할당
    private void AssignRandomColors(StartPoint[] startArray,EndPoint[] endArray)
    {
        // colors 배열을 무작위로 섞음
        ShuffleArray(colors);

        // 배열의 각 요소에 무작위 색상 할당
        for (int i = 0; i < startArray.Length; i++)
        {
            startArray[i].setColor(colors[i]); // 섞인 colors 배열의 색상을 순서대로 할당
            startArray[i].setMaterial();
            endArray[i].setColor(colors[i]);
            endArray[i].setMaterial();
        }
    }
}
