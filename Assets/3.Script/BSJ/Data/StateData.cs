using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateData : MonoBehaviour
{
    //전체
    [System.Serializable]
    public class GameState
    {
        public List<FloorState> floors; //전체 층의 상태를 담는 리스트

        //플레이어 위치
        public float playerPositionX;
        public float playerPositionY;
        public float playerPositionZ;

        //플레이어 회전
        public float playerRotationX;
        public float playerRotationY;
        public float playerRotationZ;
        public float playerRotationW;
    }

    //층별
    [System.Serializable]
    public class FloorState
    {
        public int floorIndex; // 각 층 Index
        public List<InteractableObjectState> interactableObjects; //해당 층의 오브젝트 상태 리스트
    }

    //층별 상호작용 오브젝트
    [System.Serializable]
    public class InteractableObjectState
    {
        public int objectIndex; //각 상호작용 오브젝트의 Index
        public bool isInteracted; // 상호작용 상태
    }
}
