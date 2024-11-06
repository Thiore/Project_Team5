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
    }

    //층별
    [System.Serializable]
    public class FloorState
    {
        public int[] foorIndex; // 각 층별의 Index
        public List<InteractableObjectState> interactableObjects; //해당 층의 오브젝트 상태 리스트
    }

    //층별 상호작용 오브젝트
    [System.Serializable]
    public class InteractableObjectState
    {
        public int[] objectIndex; //각 오브젝트의 Index
        public bool isInteracted; // 상호작용 상태
    }
}
