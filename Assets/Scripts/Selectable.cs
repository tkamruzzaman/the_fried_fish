using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Selectable : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    
    public bool IsSelected { get; private set; }

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
    }

    public void Select(Color selectedColor)
    {
        IsSelected = true;
        spriteRenderer.color = selectedColor;
    }

    public void Deselect()
    {
        IsSelected = false;
        spriteRenderer.color = originalColor;
    }
}