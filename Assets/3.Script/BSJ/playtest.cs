using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playtest : MonoBehaviour
{
    private Vector3 lastPosition;
    private Quaternion lastRotation;

    private void Start()
    {
        if (SaveManager.Instance != null)
        {
            //SaveManager.Instance.LoadPlayerPosition();
            Debug.Log("Final Position Applied in Player Start.");
        }
    }

    private void LateUpdate()
    {
        GameObject player = GameObject.FindGameObjectWithTag("MainPlayer");
        if (player != null)
        {
            Vector3 currentPosition = player.transform.position;
            Quaternion currentRotation = player.transform.rotation;

            // 위치나 회전 값이 변경되었는지 체크
            if (currentPosition != lastPosition || currentRotation != lastRotation)
            {
                Debug.Log($"Position changed to: {currentPosition}");
                Debug.Log($"Rotation changed to: {currentRotation}");
                lastPosition = currentPosition;
                lastRotation = currentRotation;
            }
        }
    }
}
