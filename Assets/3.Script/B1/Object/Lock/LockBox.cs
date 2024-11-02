using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockBox : MonoBehaviour
{
    private ReadInputData input;
    public GameObject[] cinemachine;

    private void Start()
    {
        TryGetComponent(out input);
        if (input == null)
        {
            Debug.LogError("ReadInputData 컴포넌트가 할당되지 않았습니다.");
            return;
        }

        Debug.Log(input);
    }

    private void Update()
    {
        if (input.isTouch)
        {
            Debug.Log("상자 디버그");
            cinemachine[0].gameObject.SetActive(true);
        }
    }

    


}
