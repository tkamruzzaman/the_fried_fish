using UnityEngine;

public class PathNode
{
    public Vector2 Position;
    public PathNode Parent;
    public float G;
    public float H;
    public float F => G + H;
    public NodeState State = NodeState.None;

    public PathNode(Vector2 pos)
    {
        Position = pos;
        State = NodeState.None;
    }
}

public enum NodeState
{
    None,
    Open,
    Closed,
    Path
}