using UnityEngine;
using System.Collections.Generic;

public class PathVisualizer : MonoBehaviour
{
    [Header("Path Visualization")]
    [SerializeField] private bool showPath = true;
    [SerializeField] private Color pathColor = Color.green;
    [SerializeField] private float pathLineWidth = 0.1f;

    private LineRenderer pathLine;

    private void Start()
    {
        SetupPathLine();
    }

    private void SetupPathLine()
    {
        pathLine = gameObject.AddComponent<LineRenderer>();
        pathLine.startWidth = pathLineWidth;
        pathLine.endWidth = pathLineWidth;
        pathLine.material = new Material(Shader.Find("Sprites/Default"));
        pathLine.startColor = pathColor;
        pathLine.endColor = pathColor;
        pathLine.sortingOrder = 1;
        ClearPath();
    }

    public void UpdatePathVisual(List<Vector2> path)
    {
        if (!showPath)
        {
            ClearPath();
            return;
        }

        pathLine.positionCount = path.Count;
        for (int i = 0; i < path.Count; i++)
        {
            pathLine.SetPosition(i, new Vector3(path[i].x, path[i].y, 0));
        }
    }

    public void ClearPath()
    {
        pathLine.positionCount = 0;
    }
}
