using UnityEngine;

public class ScalableObject : MonoBehaviour
{
    [Header("Scaling Settings")]
    public float scaleFactor = 0.5f;    // 0.5 = 50% smaller; 2.0 = 200% larger
    public float scaleDuration = 0.5f;  // Time for the smooth transition (Lerp speed)
    
    private Vector3 originalScale;
    private Vector3 targetScale;
    private bool isSmall = false;

    void Start()
    {
        // Store the starting scale from the Inspector
        originalScale = transform.localScale;
        targetScale = originalScale;
    }

    void Update()
    {
        // Smoothly transition the scale in Update()
        // Use transform.localScale directly for the current scale in the Lerp
        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.deltaTime * (1f / scaleDuration));
    }

    // Public function called by the remote's raycast
    public void ToggleScale()
    {
        if (isSmall)
        {
            // Grow back to original size
            targetScale = originalScale;
        }
        else
        {
            // Shrink/Grow by the scale factor
            // Note: If scaleFactor is 0.5, it shrinks; if it's 2.0, it grows.
            targetScale = originalScale * scaleFactor;
        }
        isSmall = !isSmall;
    }
}