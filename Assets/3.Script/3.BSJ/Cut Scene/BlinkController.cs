using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlinkController : MonoBehaviour
{
    [SerializeField] private Image uiImage;  // UI 이미지 컴포넌트
    private Material blinkMaterial;
    private string propertyName = "_Blink";
    public float propertyValue; //Blink 값
    [SerializeField] private float openValue;
    [SerializeField] private float firstValue;
    [SerializeField] private float secondValue;
    [SerializeField] private float endValue;

    private void Start()
    {
        propertyValue = 2f;

        if (uiImage != null)
        {
            Debug.Log("dd");
            blinkMaterial = uiImage.material;  // Image의 Material을 가져옴
            //StartCoroutine(Blink_co());
            //if (blinkMaterial != null && blinkMaterial.HasProperty(propertyName))
            //{
            //    //blinkMaterial.SetFloat(propertyName, propertyValue);

            //    Debug.Log("여기!");
            //}
            //else
            //{
            //    Debug.Log("이거 안들어와유");
            //}
        }
        
    }

    private void Update()
    {
        blinkMaterial.SetFloat(propertyName, propertyValue);
    }
    private IEnumerator Blink_co()
    {
        float delayTime = 0f;
        while(delayTime<1f)
        {
            delayTime += Time.deltaTime;
            propertyValue = Mathf.Lerp(openValue, firstValue, delayTime);
            blinkMaterial.SetFloat(propertyName, propertyValue);
            yield return null;
        }
        delayTime = 0f;
        while(delayTime<1f)
        {
            delayTime += Time.deltaTime;
            propertyValue = Mathf.Lerp(firstValue, openValue, delayTime);
            blinkMaterial.SetFloat(propertyName, propertyValue);
            yield return null;
        }
        delayTime = 0f;
        while (delayTime < 1f)
        {
            delayTime += Time.deltaTime;
            propertyValue = Mathf.Lerp(openValue, secondValue, delayTime);
            blinkMaterial.SetFloat(propertyName, propertyValue);
            yield return null;
        }
        delayTime = 0f;
        while (delayTime < 1f)
        {
            delayTime += Time.deltaTime;
            propertyValue = Mathf.Lerp(secondValue, openValue, delayTime);
            blinkMaterial.SetFloat(propertyName, propertyValue);
            yield return null;
        }
        delayTime = 0f;
        while (delayTime < 1f)
        {
            delayTime += Time.deltaTime;
            propertyValue = Mathf.Lerp(openValue, endValue, delayTime);
            blinkMaterial.SetFloat(propertyName, propertyValue);
            yield return null;
        }
    }
    private void SetValue(float value)
    {
        blinkMaterial.SetFloat(propertyName, value);
        propertyValue = value;
    }

    //public void SetProperty(float value)
    //{
    //    if (blinkMaterial != null && blinkMaterial.HasProperty(propertyName))
    //    {
    //        blinkMaterial.SetFloat(propertyName, value);
    //        Debug.Log($"Property '{propertyName}' set to {value}");
    //    }
    //    else
    //    {
    //        Debug.LogError($"Material or property '{propertyName}' not found!");
    //    }
    //}
}
