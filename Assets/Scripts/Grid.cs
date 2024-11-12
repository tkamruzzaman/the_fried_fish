using UnityEngine;
using System.Collections.Generic;

public class Grid : MonoBehaviour
{
    [Header("Grid Settings")]
    [SerializeField] private float gridSize = 0.5f;
    [SerializeField] private Vector2 gridOffset = Vector2.zero;
    [SerializeField] private int gridWidth = 50;
    [SerializeField] private int gridHeight = 50;
    [SerializeField] private LayerMask obstacleLayer;

    [Header("Debug Visualization")]
    [SerializeField] private bool showGrid = true;
    [SerializeField] private Color gridColor = new Color(0.5f, 0.5f, 0.5f, 0.2f);

    private List<GameObject> visualizationObjects = new List<GameObject>();

    public float GridSize => gridSize;
    public Vector2 GridOffset => gridOffset;
    public int Width => gridWidth;
    public int Height => gridHeight;

    public Vector2 GridToWorldPosition(Vector2 gridPos)
    {
        return new Vector2(
            gridPos.x * gridSize + gridOffset.x,
            gridPos.y * gridSize + gridOffset.y
        );
    }

    public Vector2 WorldToGridPosition(Vector2 worldPos)
    {
        return new Vector2(
            Mathf.Floor((worldPos.x - gridOffset.x) / gridSize),
            Mathf.Floor((worldPos.y - gridOffset.y) / gridSize)
        );
    }

    public bool IsWalkable(Vector2 position)
    {
        return !Physics2D.OverlapCircle(position, gridSize * 0.4f, obstacleLayer);
    }

    void OnDrawGizmos()
    {
        if (!showGrid) return;

        Gizmos.color = gridColor;

        // Draw vertical lines
        for (int x = 0; x <= gridWidth; x++)
        {
            Vector3 start = GridToWorldPosition(new Vector2(x, 0));
            Vector3 end = GridToWorldPosition(new Vector2(x, gridHeight));
            Gizmos.DrawLine(start, end);
        }

        // Draw horizontal lines
        for (int y = 0; y <= gridHeight; y++)
        {
            Vector3 start = GridToWorldPosition(new Vector2(0, y));
            Vector3 end = GridToWorldPosition(new Vector2(gridWidth, y));
            Gizmos.DrawLine(start, end);
        }
    }
}