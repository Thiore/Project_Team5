using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerToggleFlash : MonoBehaviour
{
    [SerializeField] private Light playerFlash;

    private Light cutSceneLight;

    private void Awake()
    {
        TryGetComponent(out cutSceneLight);
    }
    private void OnEnable()
    {
        if (playerFlash.enabled)
            cutSceneLight.enabled = true;
    }
    private void OnDisable()
    {
        if (cutSceneLight.enabled)
            cutSceneLight.enabled = false;
    }
}
