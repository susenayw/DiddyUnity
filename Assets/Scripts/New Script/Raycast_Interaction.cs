using UnityEngine;
using TMPro; // Important for TextMeshPro UI

public class Raycast_Interaction : MonoBehaviour
{
    [Header("Setup")]
    public float interactionDistance = 3f;
    public TextMeshProUGUI interactionPromptText; // Drag the TextMeshPro object here
    private Camera mainCamera;
    
    void Start()
    {
        mainCamera = Camera.main;

        if (interactionPromptText != null)
        {
            interactionPromptText.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        // 1. Fire a Raycast from the center of the camera
        Ray ray = mainCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;
        
        bool isLookingAtInteractable = false;
        DoorInteraction doorToInteract = null;
        string promptMessage = ""; // Initialize the message as empty

        if (Physics.Raycast(ray, out hit, interactionDistance))
        {
            GameObject hitObject = hit.collider.gameObject;
            
            // --- B. Door Button Interaction ---
            DoorButtonReference buttonRef = hitObject.GetComponent<DoorButtonReference>();
            
            if (buttonRef != null && buttonRef.targetDoor != null)
            {
                // We hit a button object that references a door.
                doorToInteract = buttonRef.targetDoor;
                // Set the specific prompt for the button
                promptMessage = "Press F or Square/X to USE BUTTON"; 
            }

            // --- A. Direct Door Interaction (Fallback only if we haven't found a door yet) ---
            if (doorToInteract == null && hitObject.CompareTag("Interactable"))
            {
                // Try to get the specific DoorInteraction component from the hit object itself
                doorToInteract = hitObject.GetComponent<DoorInteraction>();
                
                // If we found the door directly, the promptMessage remains the initialized empty string.
            }


            // --- C. Interaction Execution ---
            if (doorToInteract != null)
            {
                isLookingAtInteractable = true;
                interactionPromptText.gameObject.SetActive(true);
                
                // Set the prompt text based on the logic above
                interactionPromptText.text = promptMessage;

                // Check for the interaction key press (F OR Controller Square/X button)
                if (Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown(KeyCode.JoystickButton0)) 
                {
                    // CALL THE INTERACT FUNCTION ON THE DOOR
                    doorToInteract.Interact();
                }
            }
        }
        
        // If we hit nothing, hide the prompt
        if (!isLookingAtInteractable)
        {
            interactionPromptText.gameObject.SetActive(false);
        }
    }
}