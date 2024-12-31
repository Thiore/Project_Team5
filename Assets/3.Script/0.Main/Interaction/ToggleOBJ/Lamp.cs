using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lamp : MonoBehaviour
{
    [SerializeField] private bool isElectricRoom;
    private Material material;
    private Light lampLight;

    private void Awake()
    {
        TryGetComponent(out MeshRenderer renderer);
        material = renderer.material;
        transform.GetChild(0).TryGetComponent(out lampLight);

        InteractionSpinPuzzle puzzle = FindObjectOfType<InteractionSpinPuzzle>();
        puzzle.OnLamp += OnLamp;
    }
    private void Start()
    {
        if(!isElectricRoom)
        {
            if (DataSaveManager.Instance.GetGameState(1, 103))
            {
                material.EnableKeyword("_EMISSION");
                lampLight.enabled = true;
            }
            else
            {
                material.DisableKeyword ("_EMISSION");
                lampLight.enabled = false;
            }
        }
        
    }

    public void OnLamp(bool isOn)
    {
        if(isOn)
        {
            material.EnableKeyword("_EMISSION");
            lampLight.enabled = true;
        }
        else
        {
            if(lampLight.enabled)
            {
                material.DisableKeyword("_EMISSION");
                lampLight.enabled = false;
            }
            else
            {
                material.EnableKeyword("_EMISSION");
                lampLight.enabled = true;
            }
        }
    }
}
