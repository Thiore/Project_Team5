using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cylinder : MonoBehaviour
{
    public bool isRotating = false;
    private int minValue=0;
    private int maxValue = 180;
    public int correctValue;

    private void Update()
    {
        
    }

    private Quaternion SetRotateValue(float rotateValue, float rotateSpeed)
    {
        float targetRotationZ;
        if (transform.rotation.z + rotateValue > maxValue)
        {
            targetRotationZ = maxValue;
        }
        else if (transform.rotation.z + rotateValue < minValue)
        {
            targetRotationZ = minValue;
        }
        else
        {
            targetRotationZ = transform.rotation.z + rotateValue;
        }
        Quaternion targetRotation = 
            new Quaternion(transform.rotation.x, transform.rotation.y, targetRotationZ, transform.rotation.w);

        return targetRotation;
    }

    public void Rotate(float rotateValue,float rotateSpeed)
    {
        float targetRotationZ;
        if (transform.rotation.z + rotateValue > 180)
        {
            targetRotationZ = 180;
        }
        else if(transform.rotation.z+rotateValue<0)
        {
            targetRotationZ = 0;
        }
        else
        {
            targetRotationZ = transform.rotation.z + rotateValue;
        }

        Quaternion targetRotation = new Quaternion(transform.rotation.x, transform.rotation.y, 
            targetRotationZ, transform.rotation.w);
        transform.rotation = Quaternion.Lerp(transform.rotation,targetRotation,rotateSpeed);

        if (Quaternion.Angle(transform.rotation, targetRotation) < 0.1f)
        {
            transform.rotation = targetRotation;
            isRotating = false;
        }
    }
}
