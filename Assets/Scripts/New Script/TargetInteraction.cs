using UnityEngine;
using System.Collections; // Required for Coroutines

public class TargetInteraction : MonoBehaviour
{
    [Header("Visual Settings")]
    public Color hitColor = Color.red;
    public float flashDuration = 0.5f; // How long the color stays red

    private Color originalColor;
    private Renderer targetRenderer;
    private Coroutine flashCoroutine;

    void Start()
    {
        targetRenderer = GetComponent<Renderer>();
        if (targetRenderer != null)
        {
            // Store the object's initial color
            originalColor = targetRenderer.material.color;
        }
    }

    // Function called automatically when another object collides with this one
    private void OnCollisionEnter(Collision collision)
    {
        // Check if the colliding object has a Rigidbody (indicating it's a thrown/shot object)
        // This ensures the target only reacts to thrown/shot items, not the player character.
        if (collision.gameObject.GetComponent<Rigidbody>() != null)
        {
            // If the coroutine is already running (still red), stop it first
            if (flashCoroutine != null)
            {
                StopCoroutine(flashCoroutine);
            }

            // Start the visual feedback sequence
            flashCoroutine = StartCoroutine(FlashRed());
        }
    }

    IEnumerator FlashRed()
    {
        // Change the color instantly to red
        if (targetRenderer != null)
        {
            targetRenderer.material.color = hitColor;
        }

        // Wait for the specified duration (0.5 seconds)
        yield return new WaitForSeconds(flashDuration);

        // Change the color back to the original color
        if (targetRenderer != null)
        {
            targetRenderer.material.color = originalColor;
        }

        flashCoroutine = null;
    }
}