using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fuse_Binker : MonoBehaviour
{
    [SerializeField] private Material[] blinkers;
    private MeshRenderer mesh;

    private void OnEnable()
    {
        TryGetComponent(out mesh);    
    }

    public void SetBlinkerColor(int num)
    {
       mesh.material = blinkers[num];
    }

}
