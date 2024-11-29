using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using Cinemachine;
public class DepthOfFieldController : MonoBehaviour
{
    public Volume globalVolume; // Cinemachine Virtual Camera 연결
    //private VolumeProfile volumeProfile;           // Volume Profile 참조
    private DepthOfField depthOfField;             // Depth of Field 참조\
   

    private float start = 10f;
    private float finish = 1f;

    public float dof;

    void Start()
    {
        // Cinemachine Virtual Camera에서 Volume Profile을 가져오기
        //var volumeComponent = virtualCamera.GetCinemachineComponent<volumes>
        //if (volumeComponent == null)
        //{
        //    Debug.LogError("Cinemachine Virtual Camera에 연결된 Volume이 없습니다!");
        //    return;
        //}

        // Volume Profile 가져오기
        //volumeProfile = volumeComponent.sharedProfile; // 연결된 Volume Profile 참조
        //if (volumeProfile == null)
        //{
        //    Debug.LogError("Volume Profile이 Virtual Camera에 설정되지 않았습니다!");
        //    return;
        //}

        // Depth of Field 가져오기
        //if (!globalVolume.profile.TryGet<DepthOfField>(out depthOfField))
        //{
        //    Debug.LogError("Volume Profile에 Depth of Field 설정이 없습니다!");
        //}
        globalVolume.profile.TryGet(out depthOfField);

    }

    void FixedUpdate()
    {
        if (depthOfField != null)
        {
            // Depth of Field 값 수정
            //depthOfField.focalLength.value = Mathf.PingPong(Time.time * 10f, 50f); // 시간에 따라 변경

            //dof = depthOfField.focalLength.value;
            depthOfField.focalLength.value = dof;


        }
    }
}

    
