using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Selectable))]
public class MovementController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    
    private List<Vector2> currentPath;
    private int currentPathIndex;
    private bool isMoving;
    private Selectable selectable;
    private SelectionManager selectionManager;
    private PathVisualizer pathVisualizer;

    private void Start()
    {
        selectable = GetComponent<Selectable>();
        currentPath = new List<Vector2>();
        selectionManager = FindFirstObjectByType<SelectionManager>();
        pathVisualizer = FindFirstObjectByType<PathVisualizer>();

        // Subscribe to selection events
        selectionManager.OnObjectSelected.AddListener(OnObjectSelected);
        selectionManager.OnObjectDeselected.AddListener(OnObjectDeselected);
        selectionManager.OnLocationSelected.AddListener(OnLocationSelected);
    }

    private void Update()
    {
        if (isMoving && selectable.IsSelected)
        {
            MoveAlongPath();
        }
    }

    private void OnObjectSelected(Selectable selected)
    {
        if (selected == selectable)
        {
            StopMovement();
        }
    }

    private void OnObjectDeselected(Selectable deselected)
    {
        if (deselected == selectable)
        {
            StopMovement();
        }
    }

    private void OnLocationSelected(Vector2 targetLocation)
    {
        if (selectable.IsSelected)
        {
            RequestPath(targetLocation);
        }
    }

    private void RequestPath(Vector2 targetPosition)
    {
        Pathfinder pathfinder = FindFirstObjectByType<Pathfinder>();
        if (pathfinder != null)
        {
            List<Vector2> path = pathfinder.FindPath(transform.position, targetPosition);
            if (path != null && path.Count > 0)
            {
                SetPath(path);
            }
        }
    }

    private void SetPath(List<Vector2> path)
    {
        currentPath = path;
        currentPathIndex = 0;
        isMoving = true;
        pathVisualizer?.UpdatePathVisual(path);
    }

    private void MoveAlongPath()
    {
        if (currentPathIndex >= currentPath.Count)
        {
            StopMovement();
            return;
        }

        Vector2 targetPosition = currentPath[currentPathIndex];
        Vector2 currentPosition = transform.position;

        transform.position = Vector2.MoveTowards(
            currentPosition,
            targetPosition,
            moveSpeed * Time.deltaTime
        );

        if (Vector2.Distance(currentPosition, targetPosition) < 0.1f)
        {
            currentPathIndex++;
            UpdateRemainingPathVisual();
        }
    }

    private void UpdateRemainingPathVisual()
    {
        if (currentPath != null && currentPathIndex < currentPath.Count)
        {
            var remainingPath = currentPath.GetRange(currentPathIndex, currentPath.Count - currentPathIndex);
            pathVisualizer?.UpdatePathVisual(remainingPath);
        }
    }

    private void StopMovement()
    {
        isMoving = false;
        currentPath.Clear();
        currentPathIndex = 0;
        pathVisualizer?.ClearPath();
    }

    private void OnDestroy()
    {
        if (selectionManager != null)
        {
            selectionManager.OnObjectSelected.RemoveListener(OnObjectSelected);
            selectionManager.OnObjectDeselected.RemoveListener(OnObjectDeselected);
            selectionManager.OnLocationSelected.RemoveListener(OnLocationSelected);
        }
    }
}