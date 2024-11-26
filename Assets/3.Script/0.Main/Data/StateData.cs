using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public class StateData : MonoBehaviour
//{
    
//}
//전체 게임 상태
[System.Serializable]
public class GameState
{
    public int objectIndex;// 오브젝트Index
    public bool isInteracted;//isInteracted
    //public Dictionary<int, Dictionary<int, bool>> interactedState; //층별 오브젝트의 상태
    //public string selectedLocale; //현재 선택된 언어


    ////플레이어의 좌표정보
    //public float playerPositionX;
    //public float playerPositionY;
    //public float playerPositionZ;

    ////플레이어 카메라의 회전값
    //public float playerRotationX;
    //public float playerRotationY;
    //public float playerRotationZ;
    //public float playerRotationW;
}

//����
//[System.Serializable]
//public class FloorState
//{
//    public int objectIndex; // 오브젝트Index
//    public bool isInteracted; //isInteracted
//}

////���� ��ȣ�ۿ� ������Ʈ
//[System.Serializable]
//public class InteractableObjectState
//{
//    public int objectIndex; //�� ��ȣ�ۿ� ������Ʈ�� Index
//    public bool isInteracted; // ��ȣ�ۿ� ����
//}