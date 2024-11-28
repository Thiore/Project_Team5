using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using Cinemachine;
public class DepthOfFieldController : MonoBehaviour
{
    [SerializeField] private Volume volumeProfile; // 적용할 Volume Profile
    [SerializeField] private float focalLength; // 설정할 Focal Length 값
    [SerializeField] private CinemachineVirtualCamera virtualCamera; // 연결할 Cinemachine Virtual Camera

    private DepthOfField depthOfField;

    private void Start()
    {
        if (volumeProfile != null &&  volumeProfile.profile.TryGet(out depthOfField))
        {
            //초기 Focal Length 설정
            depthOfField.focalLength.value = focalLength;
        }
    }
}
