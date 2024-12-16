using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateSkybox : MonoBehaviour
{
    private void Update()
    {
        RenderSettings.skybox.SetFloat("_Rotation", Time.fixedTime);
    }
}
