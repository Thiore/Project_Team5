 using System.Collections.Generic;
 using UnityEngine;

public class Plane : MonoBehaviour
{
    public List<GameObject> tileList;
    public List<GameObject> rayStartTile;
    public Material lampMaterial;

    private void Start()
    {
        // 모든 타일에 있는 GridSpin의 회전 완료 이벤트 구독
        foreach (var tile in tileList)
        {
            GridSpin gridSpin = tile.GetComponent<GridSpin>();
            if (gridSpin != null)
            {
                gridSpin.OnRotationComplete += CheckRay; // 회전 완료 시 CheckRay 호출
            }
        }
        lampMaterial.DisableKeyword("_EMISSION");
    }



    // CheckRay 메서드: 모든 rayStartTile의 RayCheck를 확인
    private void CheckRay()
    {
        foreach (var ray in rayStartTile)
        {
            RayCheck rayCheck = ray.GetComponent<RayCheck>();
            rayCheck.CheckConnections();

            if (rayCheck.isComplete)
            {
                lampMaterial.EnableKeyword("_EMISSION");
                //gamecomplete
            }
        }
    }
}
