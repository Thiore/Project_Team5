using UnityEngine;

public class GridInitializer : MonoBehaviour
{
    public GameObject tilePrefab;       // 타일로 사용할 프리팹
    public int gridSize = 6;            // 그리드 크기 (6x6)
    public Transform gridParent;        // Grid 컴포넌트를 가진 부모 오브젝트
    public float tileSize = 1f;         // 각 타일의 크기
    public float tileSpacing = 0.1f;    // 타일 사이의 간격

    private void Start()
    {
        InitializeGrid();
    }

    private void InitializeGrid()
    {

        // 그리드의 시작 위치 설정 (왼쪽 아래로 이동)
        Vector3 startPosition = gridParent.position - new Vector3(
            (gridSize - 1) * (tileSize + tileSpacing) / 2,
            0,
            (gridSize - 1) * (tileSize + tileSpacing) / 2
        );

        // 6x6 타일 생성 및 배치
        for (int x = 0; x < gridSize; x++)
        {
            for (int z = 0; z < gridSize; z++)
            {
                // 타일 생성 및 배치
                GameObject tile = Instantiate(tilePrefab, startPosition + new Vector3(
                    x * (tileSize + tileSpacing),
                    0,
                    z * (tileSize + tileSpacing)
                ), Quaternion.identity, gridParent);

                tile.name = $"Tile_{x}_{z}"; // 타일 이름 지정
            }
        }
    }
}
