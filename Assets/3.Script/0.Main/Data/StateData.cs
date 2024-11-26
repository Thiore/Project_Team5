using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateData : MonoBehaviour
{
    //전체 게임 상태
    [System.Serializable]
    public class GameState
    {
        public Dictionary<int, List<InteractableObjectState>> floors; //층별 오브젝트의 상태
        public string selectedLocale; //현재 선택된 언어


        //�÷��̾� ��ġ
        public float playerPositionX;
        public float playerPositionY;
        public float playerPositionZ;

        //�÷��̾� ȸ��
        public float playerRotationX;
        public float playerRotationY;
        public float playerRotationZ;
        public float playerRotationW;
    }

    //����
    [System.Serializable]
    public class FloorState
    {
        public int floorIndex; // �� �� Index
        public List<InteractableObjectState> interactableObjects; //�ش� ���� ������Ʈ ���� ����Ʈ
    }

    //���� ��ȣ�ۿ� ������Ʈ
    [System.Serializable]
    public class InteractableObjectState
    {
        public int objectIndex; //�� ��ȣ�ۿ� ������Ʈ�� Index
        public bool isInteracted; // ��ȣ�ۿ� ����
    }
}
