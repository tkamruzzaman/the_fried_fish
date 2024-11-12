using UnityEngine;
using UnityEngine.Events;

public class SelectionManager : MonoBehaviour
{
    [Header("Selection Settings")]
    [SerializeField] private LayerMask selectableLayer;
    [SerializeField] private Color selectedColor = Color.yellow;

    private Camera mainCamera;
    private Selectable selectedObject;

    public UnityEvent<Selectable> OnObjectSelected;
    public UnityEvent<Selectable> OnObjectDeselected;
    public UnityEvent<Vector2> OnLocationSelected;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            HandleSelection();
        }
    }

    private void HandleSelection()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity, selectableLayer);

        if (hit.collider != null)
        {
            Selectable selectable = hit.collider.GetComponent<Selectable>();
            if (selectable != null)
            {
                if (selectable == selectedObject)
                {
                    DeselectCurrentObject();
                }
                else
                {
                    SelectNewObject(selectable);
                }
            }
        }
        else if (selectedObject != null)
        {
            Vector2 targetPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            OnLocationSelected?.Invoke(targetPos);
        }
    }

    private void SelectNewObject(Selectable selectable)
    {
        if (selectedObject != null)
        {
            DeselectCurrentObject();
        }

        selectedObject = selectable;
        selectedObject.Select(selectedColor);
        OnObjectSelected?.Invoke(selectedObject);
    }

    private void DeselectCurrentObject()
    {
        if (selectedObject != null)
        {
            OnObjectDeselected?.Invoke(selectedObject);
            selectedObject.Deselect();
            selectedObject = null;
        }
    }
}