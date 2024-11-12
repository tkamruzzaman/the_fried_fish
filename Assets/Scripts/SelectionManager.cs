using UnityEngine;

public class SelectionManager : MonoBehaviour
{
    private GameObject selectedObject;
    private Camera mainCamera;
    
    // The layer that contains selectable objects
    [SerializeField] private LayerMask selectableLayer;
    
    // Visual indicator for selected object
    [SerializeField] private Color selectedColor = Color.yellow;
    private Color originalColor;
    
    void Start()
    {
        mainCamera = Camera.main;
    }
    
    void Update()
    {
        // Handle left mouse click
        if (Input.GetMouseButtonDown(0))
        {
            HandleSelection();
        }
    }
    
    void HandleSelection()
    {
        print("ssssssss");
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity, selectableLayer);
        
        // If we hit something
        if (hit.collider != null)
        {
            // If we click the currently selected object, deselect it
            if (hit.collider.gameObject == selectedObject)
            {
                DeselectCurrentObject();
                return;
            }
            
            // If we click a new object while having one selected, deselect the old one
            if (selectedObject != null)
            {
                DeselectCurrentObject();
            }
            
            // Select the new object
            selectedObject = hit.collider.gameObject;
            
            // Store original color and change to selected color
            SpriteRenderer spriteRenderer = selectedObject.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                originalColor = spriteRenderer.color;
                spriteRenderer.color = selectedColor;
            }
        }
        // If we click empty space and have something selected
        else if (selectedObject != null)
        {
            // Move the selected object to the clicked position
            Vector3 worldPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            worldPosition.z = 0; // Keep the z-position consistent for 2D
            selectedObject.transform.position = worldPosition;
        }
    }
    
    void DeselectCurrentObject()
    {
        if (selectedObject != null)
        {
            // Restore original color
            SpriteRenderer spriteRenderer = selectedObject.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                spriteRenderer.color = originalColor;
            }
            
            selectedObject = null;
        }
    }
}
