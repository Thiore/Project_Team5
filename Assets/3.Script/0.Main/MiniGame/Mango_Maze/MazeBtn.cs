using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeBtn : MonoBehaviour
{
    public GameObject plane;

    private const string BodySensorsPermission = "android.permission.BODY_SENSORS";

    public void enablePlane()
    {
        plane.SetActive(true);
        //plane.GetComponent<PlaneTiltController>().GetAccelerometer();

        // 권한이 있는지 확인
        if (!UnityEngine.Android.Permission.HasUserAuthorizedPermission(BodySensorsPermission))
        {
            Debug.Log("BODY_SENSORS 권한이 없습니다. 요청 중...");
            UnityEngine.Android.Permission.RequestUserPermission(BodySensorsPermission);
        }
        else
        {
            Debug.Log("BODY_SENSORS 권한이 이미 부여되었습니다.");
        }
    }
    
}