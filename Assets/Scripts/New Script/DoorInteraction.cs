using UnityEngine;

public class DoorInteraction : MonoBehaviour
{
    // Public variables
    public bool isOpen = false;
    [Header("Sliding Settings")]
    public float slideDistance = 2f; // How far the door slides (e.g., 2 units)
    public float speed = 5f;        // Use a slightly higher speed for position lerping
    
    private Vector3 closedPosition;
    private Vector3 openPosition;

    void Start()
    {
        // Store the door's starting position as the "closed" state
        closedPosition = transform.localPosition;
        
        // Calculate the "open" position. We assume the door slides along its RIGHT (X) axis.
        // If your door needs to slide along Z (forward) or Y (up/down), change Vector3.right to 
        // Vector3.forward or Vector3.up, respectively.
        openPosition = closedPosition + (transform.right * slideDistance); 
    }

    void Update()
    {
        // Smoothly move the door towards the target position (open or closed)
        Vector3 targetPosition = isOpen ? openPosition : closedPosition;
        
        // Use Vector3.Lerp for a smooth, time-based movement
        transform.localPosition = Vector3.Lerp(
            transform.localPosition, 
            targetPosition, 
            Time.deltaTime * speed
        );
    }

    // Interact function remains the same
    public void Interact()
    {
        // Toggle the open state
        isOpen = !isOpen;
        
        // Play a sound or particles here if you wish
        Debug.Log(gameObject.name + " was toggled. Open: " + isOpen);
    }
}