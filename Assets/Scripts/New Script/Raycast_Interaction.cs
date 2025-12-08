using UnityEngine;
using TMPro;

public class Raycast_Interaction : MonoBehaviour
{
    [Header("Setup")]
    public float interactionDistance = 3f;
    public TextMeshProUGUI interactionPromptText; 
    private Camera mainCamera;
    
    // REFERENCES
    private FPC_Controller fpcController; 
    // NEW: The Item currently being held
    private ItemPickup currentHeldItem = null;
    // NEW: The Transform where the item should be held (must be assigned in Inspector)
    public Transform itemHoldParent; 

    void Start()
    {
        mainCamera = Camera.main;

        if (interactionPromptText != null)
        {
            interactionPromptText.gameObject.SetActive(false);
        }
        
        // Find the FPC_Controller component on the player hierarchy once
        fpcController = FindObjectOfType<FPC_Controller>();

        // Fallback for item hold parent if not set in Inspector
        if (itemHoldParent == null && mainCamera != null)
        {
            itemHoldParent = mainCamera.transform;
        }
    }

    void Update()
    {
        // --- 0. DROP LOGIC (Highest Priority) ---
        // Player is holding an item and presses Q.
        if (currentHeldItem != null && Input.GetKeyDown(KeyCode.Q))
        {
            DropItem();
            return; // Exit Update() immediately after dropping
        }

        Ray ray = mainCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;
        
        bool isLookingAtInteractable = false;
        MonoBehaviour interactableTarget = null; 
        string promptMessage = "";
        
        // Determine player seating state
        bool playerIsSeated = fpcController != null && fpcController.IsSeated();


        if (Physics.Raycast(ray, out hit, interactionDistance))
        {
            GameObject hitObject = hit.collider.gameObject;

            // --- A. Item Pickup Check (NEW HIGHEST PRIORITY) ---
            ItemPickup pickup = hitObject.GetComponent<ItemPickup>();
            if (pickup != null && currentHeldItem == null && !playerIsSeated)
            {
                // Found an item to pick up, and we aren't holding anything
                interactableTarget = pickup;
                promptMessage = "Press F to pick up";
            }

            // --- B. Seat Interaction ---
            SeatInteraction seat = hitObject.GetComponent<SeatInteraction>();
            // Only check if no other target is found, player isn't seated, and seat is available
            if (interactableTarget == null && seat != null && !playerIsSeated && seat.IsAvailable())
            {
                interactableTarget = seat;
                promptMessage = "Press F to seat";
            }
            
            // --- C. Oil Spawner Button ---
            OilSpawner oilSpawner = hitObject.GetComponent<OilSpawner>();
            if (interactableTarget == null && oilSpawner != null && !playerIsSeated)
            {
                interactableTarget = oilSpawner;
                promptMessage = "Press F or Square/X to SPAWN OIL";
            }

            // --- D. Door Button Interaction ---
            DoorButtonReference doorButtonRef = hitObject.GetComponent<DoorButtonReference>();
            if (interactableTarget == null && doorButtonRef != null && doorButtonRef.targetDoor != null && !playerIsSeated)
            {
                interactableTarget = doorButtonRef.targetDoor;
                promptMessage = "Press F or Square/X to USE BUTTON"; 
            }

            // --- E. Direct Door Interaction ---
            if (interactableTarget == null && hitObject.CompareTag("Interactable") && !playerIsSeated)
            {
                DoorInteraction doorToInteract = hitObject.GetComponent<DoorInteraction>();
                if (doorToInteract != null)
                {
                    interactableTarget = doorToInteract;
                    // Prompt remains empty for direct door interaction
                }
            }


            // --- F. Interaction Prompt Check ---
            if (interactableTarget != null)
            {
                isLookingAtInteractable = true;
            }
        }
        
        // Check for Stand Up command (this happens regardless of where the raycast hits)
        if (playerIsSeated)
        {
            isLookingAtInteractable = true; // Keep prompt visible when seated
            promptMessage = "Press F to stand up";
        }


        // Display Prompt
        if (isLookingAtInteractable)
        {
            interactionPromptText.gameObject.SetActive(true);
            interactionPromptText.text = promptMessage;
        }
        else
        {
            interactionPromptText.gameObject.SetActive(false);
        }

        // --- Interaction Key Press Logic (F or Square/X) ---
        if (Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown(KeyCode.JoystickButton0)) 
        {
            // Case 1: Player is seated -> Must stand up
            if (playerIsSeated)
            {
                StandUp();
            }
            // Case 2: Player is standing and looking at an interactable object
            else if (interactableTarget != null)
            {
                // Dispatch interaction based on target type
                if (interactableTarget is ItemPickup pickup)
                {
                    PickUpItem(pickup); 
                }
                else if (interactableTarget is SeatInteraction seat)
                {
                    SitDown(seat);
                }
                else if (interactableTarget is OilSpawner spawner)
                {
                    spawner.SpawnOil(); 
                }
                else if (interactableTarget is DoorInteraction door)
                {
                    door.Interact();
                }
            }
        }
    }
    
    // --- Helper Methods ---

    void StandUp()
    {
        GameObject playerBody = fpcController.playerBody.gameObject; 
        SeatInteraction activeSeat = FindObjectOfType<SeatInteraction>(true); 
        if (activeSeat != null && activeSeat.currentSeatedPlayer == playerBody)
        {
            activeSeat.ToggleSeat(playerBody);
        }
    }

    void SitDown(SeatInteraction seat)
    {
        GameObject playerBody = fpcController.playerBody.gameObject; 
        seat.ToggleSeat(playerBody);
    }

    void PickUpItem(ItemPickup item)
    {
        if (itemHoldParent == null)
        {
            Debug.LogError("Item Hold Parent not assigned! Cannot pick up item.");
            return;
        }
        
        currentHeldItem = item;
        item.PickUp(itemHoldParent);
    }
    
    void DropItem()
    {
        if (currentHeldItem == null) return;

        // Reset parenting to the world
        currentHeldItem.transform.SetParent(null);

        // Position slightly in front of the camera (you may adjust this distance)
        currentHeldItem.transform.position = mainCamera.transform.position + mainCamera.transform.forward * 0.5f;

        // Set the state in the ItemPickup script (enables physics/collider, disables hand)
        currentHeldItem.SetInHand(false);
        
        Rigidbody rb = currentHeldItem.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.AddForce(mainCamera.transform.forward * 5f, ForceMode.Impulse); 
        }

        currentHeldItem = null;
        // Ensure the prompt is hidden
        interactionPromptText.gameObject.SetActive(false);
    }
}