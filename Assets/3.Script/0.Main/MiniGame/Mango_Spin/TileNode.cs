 using System.Collections;
 using System.Collections.Generic;
 using UnityEngine;
public enum NodePosition
{
    Top,
    Bottom,
    Left,
    Right
}
public enum NodeShape
{
    Straight,
    Slash,
    T,
    X
}

[System.Serializable]
public class Node
{
    public NodePosition nodePosition;
    public bool isOpen;
}

[ExecuteInEditMode]
public class TileNode : MonoBehaviour
{
    [SerializeField] private List<Node> nodeList = new List<Node>();
    [SerializeField] private NodeShape nodeShape;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        foreach(var nodeState in nodeList)
        {
            if (nodeState.isOpen)
            {
                Vector3 nodePosition = GetNodePosition(nodeState.nodePosition);
                Gizmos.DrawSphere(nodePosition, 0.1f);
            }
        }
    }

    private Vector3 GetNodePosition(NodePosition node)
    {
        Vector3 localNodePosition;
        switch (node)
        {
            case NodePosition.Top:
                localNodePosition = new Vector3(0, 0.25f, 0.75f);
                break;
            case NodePosition.Bottom:
                localNodePosition = new Vector3(0, 0.25f, -0.75f);
                break;
            case NodePosition.Left:
                localNodePosition= new Vector3(-0.75f, 0.25f, 0);
                break;
            case NodePosition.Right:
                localNodePosition = new Vector3(0.75f, 0.25f, 0);
                break;
            default:
                localNodePosition = Vector3.zero;
                break;
        }
        return transform.position+(transform.rotation*localNodePosition);
    }
}
