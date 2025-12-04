using UnityEngine;

public class DoorController : MonoBehaviour
{
    // The amount of time the door takes to open or close
    public float openSpeed = 2.0f;

    // The final local position when the door is fully open. 
    // You will set this in the Unity Inspector. 
    // Example: If the door is a wall segment that slides to the side, this might be Vector3(0, 0, -3).
    public Vector3 openPosition;

    // The starting local position when the door is fully closed. 
    // This is captured automatically in Start().
    private Vector3 closedPosition;

    // Tracks the current target position (open or closed)
    private Vector3 targetPosition;

    // Tracks the current state of the door
    private bool isOpen = false;

    void Start()
    {
        // Set the initial closed position of the door
        closedPosition = transform.localPosition;
        targetPosition = closedPosition;
    }

    void Update()
    {
        // Smoothly move the door towards its target position (openPosition or closedPosition)
        // Lerp moves from current position towards target over time
        transform.localPosition = Vector3.Lerp(transform.localPosition, targetPosition, Time.deltaTime * openSpeed);
    }

    // Public method called by the Player Interaction script
    public void ToggleDoor()
    {
        isOpen = !isOpen; // Toggle the state

        if (isOpen)
        {
            // Set the target to the open position
            targetPosition = openPosition;
            Debug.Log("Door opened!");
            // Optional: Add sound effect here, e.g., GetComponent<AudioSource>().Play();
        }
        else
        {
            // Set the target back to the closed position
            targetPosition = closedPosition;
            Debug.Log("Door closed!");
            // Optional: Add sound effect here
        }
    }
}