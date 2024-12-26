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
        else
            cutSceneLight.enabled = false;
    }
    private void Update()
    {
        transform.position = Camera.main.transform.position;
        transform.rotation = Camera.main.transform.rotation;
    }
    private void OnDisable()
    {
        cutSceneLight.enabled = false;
    }
}
