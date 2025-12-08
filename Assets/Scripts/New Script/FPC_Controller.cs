using UnityEngine;

public class FPC_Controller : MonoBehaviour
{
    // NEW: Crouch Variables
    [Header("Crouch Settings")]
    public float crouchHeight = 1.0f;           // Target height of the CharacterController when crouched
    private float originalCCHeight;             // Stores the Character Controller's height at startup
    private Vector3 originalCCCenter;           // Stores the Character Controller's center at startup
    private float targetCameraY;                // The final local Y position of the camera/container
    private float originalCameraY;              // Stores the starting Y position (e.g., 0.555f)
    public float crouchSpeedMultiplier = 0.5f; 
    public float cameraTransitionSpeed = 10f; 
    [HideInInspector]
    public bool isCrouched = false;

    // State variable controlled by SeatInteraction.cs
    [HideInInspector]
    public bool isSeated = false;
    
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float runSpeed = 10f; 
    public float gravity = -9.81f * 6f; 
    public float jumpHeight = 3f;
    private CharacterController characterController; 
    
    // Gravity variables
    Vector3 velocity;
    
    [Header("Ground Check")]
    public Transform groundCheck; 
    public float groundDistance = 0.4f;
    public LayerMask groundMask; 

    [Header("Camera Look")]
    public float mouseSensitivity = 2f; 
    public float lookSensitivityController = 300f; 
    public Transform playerBody; 
    float xRotation = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        characterController = GetComponentInParent<CharacterController>();
        
        if (characterController != null)
        {
            // FIX: Store the settings exactly as they are configured in the Inspector
            originalCameraY = transform.localPosition.y;
            originalCCHeight = characterController.height; 
            originalCCCenter = characterController.center;
            targetCameraY = originalCameraY; 
        }
    }

    void Update()
    {
        if (characterController == null) return; 
        
        // 0. --- CROUCH INPUT ---
        if (!isSeated)
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                isCrouched = !isCrouched;
                ApplyCrouchState();
            }
            else if (isCrouched && (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.C)))
            {
                isCrouched = false;
                ApplyCrouchState();
            }
            
            // Smoothly move the camera to the target Y position
            transform.localPosition = Vector3.Lerp(
                transform.localPosition, 
                new Vector3(transform.localPosition.x, targetCameraY, transform.localPosition.z), 
                Time.deltaTime * cameraTransitionSpeed
            );
        }

        // 1. --- MOVEMENT AND GRAVITY ---
        if (!isSeated)
        {
            bool isControllerGrounded = characterController.isGrounded;

            if (isControllerGrounded && velocity.y < 0)
            {
                velocity.y = -0.5f; 
            }
            
            // --- JUMP ---
            bool canJump = !isCrouched;

            if (canJump && (Input.GetButtonDown("Jump") || Input.GetKeyDown(KeyCode.JoystickButton3)) && isControllerGrounded)
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            }

            velocity.y += gravity * Time.deltaTime;
            
            // --- MOVEMENT ---
            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");

            float currentSpeed = moveSpeed;
            
            if (isCrouched)
            {
                currentSpeed *= crouchSpeedMultiplier;
            }
            else if (Input.GetKey(KeyCode.LeftShift) && (x != 0 || z != 0))
            {
                currentSpeed = runSpeed;
            }

            Vector3 move = playerBody.right * x + playerBody.forward * z;
            characterController.Move((move * currentSpeed * Time.deltaTime) + (velocity * Time.deltaTime));
        }

        // 2. --- CAMERA LOOK (Remains the same) ---
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");
        
        float finalMouseX = mouseX * mouseSensitivity;
        float finalMouseY = mouseY * mouseSensitivity;

        xRotation -= finalMouseY;
        
        float maxVerticalAngle = isSeated ? 45f : 90f;
        xRotation = Mathf.Clamp(xRotation, -maxVerticalAngle, maxVerticalAngle);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        
        if (isSeated)
        {
             playerBody.Rotate(Vector3.up * finalMouseX);
        }
        else 
        {
             playerBody.Rotate(Vector3.up * finalMouseX);
        }
    }
    
    // Function to handle the CharacterController height change and camera repositioning
    private void ApplyCrouchState()
    {
        if (characterController == null) return;
        
        if (isCrouched)
        {
            float heightDifference = originalCCHeight - crouchHeight;

            // 1. Set new height
            characterController.height = crouchHeight;
            
            // 2. Adjust center: Move center down by half the height difference to keep capsule bottom grounded
            characterController.center = originalCCCenter - Vector3.up * (heightDifference / 2f);
            
            // 3. Adjust camera: Move camera down by the full height difference
            targetCameraY = originalCameraY - heightDifference;
        }
        else // Stand Up
        {
            // 1. Restore original height
            characterController.height = originalCCHeight;
            
            // 2. Restore original center
            characterController.center = originalCCCenter;
            
            // 3. Restore camera position
            targetCameraY = originalCameraY;
        }
    }
    
    // Public helper function for the Raycast script to check seating state
    public bool IsSeated()
    {
        return isSeated;
    }
}