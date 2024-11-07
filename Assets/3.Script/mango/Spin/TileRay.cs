using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class TileRay : MonoBehaviour
{
    public List<RaycastDirection> rayList = new List<RaycastDirection>();
    private Vector3 origin;
    private void Start()
    {
        origin = transform.position;  
    }

    private Vector3 GetRayDirection(RaycastDirection ray)
    {

        switch (ray.direction)
        {
            case Direction.Top:
                return transform.forward;
            case Direction.Bottom:
                return -transform.forward;
            case Direction.Left:
                return -transform.right;
            case Direction.Right:
                return transform.right;
            default:
                return Vector3.forward;
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        foreach(var ray in rayList)
        {
            Vector3 direction = GetRayDirection(ray);
            Gizmos.DrawRay(origin+new Vector3(0,0.25f,0), direction.normalized * 1f);
        }
    }

    public List<GameObject> GetHitObject()
    {
        List<GameObject> hitObjects = new List<GameObject>();
        foreach (var ray in rayList)
        {
            Vector3 direction = GetRayDirection(ray);
            // Raycast 실행하여 충돌한 객체가 있는지 확인
            if (Physics.Raycast(origin, direction, out RaycastHit hit, 1f))
            {
                if (hit.collider != null)
                {
                    // 충돌한 오브젝트를 리스트에 추가
                    hitObjects.Add(hit.collider.gameObject);
                }
            }
        }
        return hitObjects;
    }
    
}
[System.Serializable]
public class RaycastDirection
{
    public Direction direction;
}
public enum Direction
{
    Top,
    Bottom,
    Left,
    Right
}
