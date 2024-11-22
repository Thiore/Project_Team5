 using System.Collections.Generic;
 using UnityEngine;

public enum TileType { Straight, TShape, LShape, DoubleLShape }

public class Tile : MonoBehaviour
{
    public TileType tileType;
    public int rotationState; // 0, 1, 2, 3 for 0°, 90°, 180°, 270°

    private Dictionary<int, bool[]> connectionMap;

    private void Start()
    {
        InitializeConnectionMap();
    }

    private void InitializeConnectionMap()
    {
        // 각 회전 상태별로 연결 가능 방향을 정의 (각각 [Top, Bottom, Left, Right] 순서)
        connectionMap = new Dictionary<int, bool[]>();

        switch (tileType)
        {
            case TileType.Straight:
                connectionMap[0] = new[] { true, true, false, false };
                connectionMap[1] = new[] { false, false, true, true };
                break;

            case TileType.TShape:
                connectionMap[0] = new[] { true, false, true, true };
                connectionMap[1] = new[] { true, true, false, true };
                connectionMap[2] = new[] { false, true, true, true };
                connectionMap[3] = new[] { true, true, true, false };
                break;

            case TileType.LShape:
                connectionMap[0] = new[] { true, false, true, false };
                connectionMap[1] = new[] { false, true, true, false };
                connectionMap[2] = new[] { false, true, false, true };
                connectionMap[3] = new[] { true, false, false, true };
                break;

            case TileType.DoubleLShape:
                connectionMap[0] = new[] { true, false, false, true };
                connectionMap[1] = new[] { false, true, true, false };
                connectionMap[2] = new[] { false, true, false, true };
                connectionMap[3] = new[] { true, false, true, false };
                break;
        }
    }

    public bool[] GetCurrentConnections()
    {
        return connectionMap[rotationState];
    }
}
