using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCheck : MonoBehaviour
{
    public SpinTile startingSpinTile; // 시작 오브젝트의 SpinTile 컴포넌트
    public List<SpinTile> targetObjects; // 모든 타겟 오브젝트 리스트
    public bool isComplete { get; private set; }
    [HideInInspector]
    public List<SpinTile> connectedObjects;

    // 연결 상태를 확인하는 메서드
    public bool CheckConnections()
    {
        if (startingSpinTile == null)
        {
            //Debug.LogError("Starting SpinTile is not assigned!");
            return false;
        }

        connectedObjects.Clear(); // 클래스 변수 연결 목록을 초기화
        RecursiveConnectionCheck(startingSpinTile, null, connectedObjects);

        // 시작 타일의 연결을 확인
        isComplete = AreAllTargetsConnected(connectedObjects);
        return isComplete;
    }

    // 재귀적으로 연결 상태 확인 (양방향 연결 필수)
    private void RecursiveConnectionCheck(SpinTile SpinTile, GameObject previousObject, List<SpinTile> connectedObjects)
    {
        connectedObjects.Add(SpinTile); // 현재 오브젝트를 연결 목록에 추가

        List<SpinTile> hitObjects = SpinTile.GetHitObject(); // 현재 오브젝트가 감지한 연결된 오브젝트 목록

        foreach (var hitObject in hitObjects)
        {
            //Debug.Log($"hit object : {hitObject.name}");
            // 현재 hitObject가 이전 오브젝트와의 양방향 연결을 확인
            SpinTile hitSpinTile = hitObject.GetComponent<SpinTile>();

            if (hitSpinTile != null && !connectedObjects.Contains(hitObject))
            {
                // hitObject가 이전 오브젝트와 연결이 되어 있는지 확인
                if (previousObject == null || IsMutuallyConnected(hitSpinTile, SpinTile))
                {
                    RecursiveConnectionCheck(hitSpinTile, SpinTile.gameObject, connectedObjects);
                }
            }
        }
    }

    // 양방향 연결 확인
    private bool IsMutuallyConnected(SpinTile SpinTile, SpinTile targetObject)
    {
        List<SpinTile> hitObjects = SpinTile.GetHitObject();
        return hitObjects.Contains(targetObject);
    }

    // 연결된 오브젝트가 모든 타겟 오브젝트와 일치하는지 확인
    private bool AreAllTargetsConnected(List<SpinTile> connectedObjects)
    {
        foreach (var target in targetObjects)
        {
            if (!connectedObjects.Contains(target))
            {
                //Debug.Log("Not all target objects are connected.");
                isComplete = false;
                return false; // 타겟 오브젝트 중 연결되지 않은 것이 있음
            }
        }

        //Debug.Log("All target objects are successfully connected!");
        this.connectedObjects = connectedObjects;
        isComplete = true;
        return true; // 모든 타겟 오브젝트가 연결됨
    }
}
