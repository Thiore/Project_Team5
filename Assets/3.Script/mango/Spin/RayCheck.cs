using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCheck : MonoBehaviour
{
    public TileRay startingTileRay; // 시작 오브젝트의 TileRay 컴포넌트
    public List<GameObject> targetObjects; // 모든 타겟 오브젝트 리스트
    public bool isComplete;
    public List<GameObject> connectedObjects;

    // 연결 상태를 확인하는 메서드
    public bool CheckConnections()
    {
        if (startingTileRay == null)
        {
            Debug.LogError("Starting TileRay is not assigned!");
            return false;
        }

        connectedObjects.Clear(); // 클래스 변수 연결 목록을 초기화
        RecursiveConnectionCheck(startingTileRay, null, connectedObjects);

        // 시작 타일의 연결을 확인
        isComplete = AreAllTargetsConnected(connectedObjects);
        return isComplete;
    }

    // 재귀적으로 연결 상태 확인 (양방향 연결 필수)
    private void RecursiveConnectionCheck(TileRay tileRay, GameObject previousObject, List<GameObject> connectedObjects)
    {
        connectedObjects.Add(tileRay.gameObject); // 현재 오브젝트를 연결 목록에 추가

        List<GameObject> hitObjects = tileRay.GetHitObject(); // 현재 오브젝트가 감지한 연결된 오브젝트 목록

        foreach (var hitObject in hitObjects)
        {
            Debug.Log($"hit object : {hitObject.name}");
            // 현재 hitObject가 이전 오브젝트와의 양방향 연결을 확인
            TileRay hitTileRay = hitObject.GetComponent<TileRay>();

            if (hitTileRay != null && !connectedObjects.Contains(hitObject))
            {
                // hitObject가 이전 오브젝트와 연결이 되어 있는지 확인
                if (previousObject == null || IsMutuallyConnected(hitTileRay, tileRay.gameObject))
                {
                    RecursiveConnectionCheck(hitTileRay, tileRay.gameObject, connectedObjects);
                }
            }
        }
    }

    // 양방향 연결 확인
    private bool IsMutuallyConnected(TileRay tileRay, GameObject targetObject)
    {
        List<GameObject> hitObjects = tileRay.GetHitObject();
        return hitObjects.Contains(targetObject);
    }

    // 연결된 오브젝트가 모든 타겟 오브젝트와 일치하는지 확인
    private bool AreAllTargetsConnected(List<GameObject> connectedObjects)
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

        Debug.Log("All target objects are successfully connected!");
        this.connectedObjects = connectedObjects;
        isComplete = true;
        return true; // 모든 타겟 오브젝트가 연결됨
    }
}
