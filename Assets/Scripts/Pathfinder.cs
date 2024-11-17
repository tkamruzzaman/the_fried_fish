using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class Pathfinder : MonoBehaviour
{
    [Header("Pathfinding Visualization")]
    [SerializeField] private bool showExploredNodes = true;
    [SerializeField] private Color openNodeColor = new Color(0, 1, 0, 0.3f);
    [SerializeField] private Color closedNodeColor = new Color(1, 0, 0, 0.3f);
    [SerializeField] private Color pathColor = Color.green;

   [SerializeField] private Grid grid;
    private List<GameObject> visualizationObjects = new List<GameObject>();
    public event System.Action<List<Vector2>> OnPathFound;

    private void Awake()
    {
        grid = FindFirstObjectByType<Grid>();
    }

    private void Start()
    {
        if (showExploredNodes)
        {
            CreateVisualizationObjects();
        }
    }

    public List<Vector2> FindPath(Vector2 start, Vector2 end)
    {
        ClearNodeVisualizations();
        var openSet = new List<PathNode>();
        var closedSet = new HashSet<Vector2>();

        var startNode = new PathNode(start);
        startNode.G = 0;
        startNode.H = Vector2.Distance(start, end);

        openSet.Add(startNode);
        UpdateNodeVisualization(start, NodeState.Open);

        while (openSet.Count > 0)
        {
            var current = openSet.OrderBy(x => x.F).First();

            if (Vector2.Distance(current.Position, end) < grid.GridSize)
            {
                var path = ReconstructPath(current);
                foreach (var pos in path)
                {
                    UpdateNodeVisualization(pos, NodeState.Path);
                }
                OnPathFound?.Invoke(path);
                return path;
            }

            openSet.Remove(current);
            closedSet.Add(current.Position);
            UpdateNodeVisualization(current.Position, NodeState.Closed);

            foreach (var neighbor in GetNeighbors(current.Position))
            {
                if (closedSet.Contains(neighbor))
                    continue;

                float newG = current.G + Vector2.Distance(current.Position, neighbor);

                var neighborNode = openSet.FirstOrDefault(x => x.Position == neighbor);

                if (neighborNode == null)
                {
                    neighborNode = new PathNode(neighbor);
                    neighborNode.G = newG;
                    neighborNode.H = Vector2.Distance(neighbor, end);
                    neighborNode.Parent = current;
                    openSet.Add(neighborNode);
                    UpdateNodeVisualization(neighbor, NodeState.Open);
                }
                else if (newG < neighborNode.G)
                {
                    neighborNode.G = newG;
                    neighborNode.Parent = current;
                }
            }
        }

        return null;
    }

    private List<Vector2> GetNeighbors(Vector2 position)
    {
        var neighbors = new List<Vector2>();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0) continue;

                Vector2 neighbor = position + new Vector2(x * grid.GridSize, y * grid.GridSize);

                if (grid.IsWalkable(neighbor))
                {
                    neighbors.Add(neighbor);
                }
            }
        }

        return neighbors;
    }

    private List<Vector2> ReconstructPath(PathNode endNode)
    {
        var path = new List<Vector2>();
        var current = endNode;

        while (current != null)
        {
            path.Add(current.Position);
            current = current.Parent;
        }

        path.Reverse();
        return path;
    }

    private void CreateVisualizationObjects()
    {
        for (int x = 0; x < grid.Width; x++)
        {
            for (int y = 0; y < grid.Height; y++)
            {
                Vector2 worldPos = grid.GridToWorldPosition(new Vector2(x, y));
                GameObject visualObj = CreateVisualizationCube(worldPos);
                visualizationObjects.Add(visualObj);
            }
        }
    }

    private GameObject CreateVisualizationCube(Vector2 position)
    {
        GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Quad);
        obj.transform.position = new Vector3(position.x, position.y, 1);
        obj.transform.localScale = new Vector3(grid.GridSize * 0.9f, grid.GridSize * 0.9f, 1);

        Material mat = new Material(Shader.Find("Sprites/Default"));
        mat.color = Color.clear;
        obj.GetComponent<MeshRenderer>().material = mat;

        Destroy(obj.GetComponent<Collider>());
        return obj;
    }

    private void UpdateNodeVisualization(Vector2 position, NodeState state)
    {
        if (!showExploredNodes) return;

        Vector2 gridPos = grid.WorldToGridPosition(position);
        int index = Mathf.FloorToInt(gridPos.x + gridPos.y * grid.Width);

        if (index >= 0 && index < visualizationObjects.Count)
        {
            Material mat = visualizationObjects[index].GetComponent<MeshRenderer>().material;

            switch (state)
            {
                case NodeState.Open:
                    mat.color = openNodeColor;
                    break;
                case NodeState.Closed:
                    mat.color = closedNodeColor;
                    break;
                case NodeState.Path:
                    mat.color = pathColor;
                    break;
                case NodeState.None:
                    mat.color = Color.clear;
                    break;
            }
        }
    }

    private void ClearNodeVisualizations()
    {
        foreach (var obj in visualizationObjects)
        {
            if (obj != null)
            {
                obj.GetComponent<MeshRenderer>().material.color = Color.clear;
            }
        }
    }

    private void OnDestroy()
    {
        foreach (var obj in visualizationObjects)
        {
            if (obj != null)
            {
                Destroy(obj);
            }
        }
    }
}
