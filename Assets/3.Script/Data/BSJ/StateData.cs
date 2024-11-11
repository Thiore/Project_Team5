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
        [HideInInspector]
        public float[] audioSound;
        [HideInInspector]
        public float[] playerPos;
        [HideInInspector]
        public float[] playerRot;
        public GameState(List<FloorState> floors, float[] audioSound, Vector3 playerPos, Quaternion playerRot)
        {
            this.floors = floors;
            this.audioSound = audioSound;
            this.playerPos = new float[3];
            this.playerPos[0] = playerPos.x;
            this.playerPos[1] = playerPos.y;
            this.playerPos[2] = playerPos.z;

            this.playerRot = new float[4];
            this.playerRot[0] = playerRot.x;
            this.playerRot[1] = playerRot.y;
            this.playerRot[2] = playerRot.z;
            this.playerRot[3] = playerRot.w;
        }
        public GameState(List<FloorState> floors, Vector3 playerPos, Quaternion playerRot)
        {
            this.floors = floors;

            this.playerPos = new float[3];
            this.playerPos[0] = playerPos.x;
            this.playerPos[1] = playerPos.y;
            this.playerPos[2] = playerPos.z;

            this.playerRot = new float[4];
            this.playerRot[0] = playerRot.x;
            this.playerRot[1] = playerRot.y;
            this.playerRot[2] = playerRot.z;
            this.playerRot[3] = playerRot.w;
        }
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
