using UnityEngine;

public class FPC_Controller : MonoBehaviour
{
    // NEW: Crouch Variables
    [Header("Crouch Settings")]
    public float crouchHeight = 1.0f;
    private float originalCCHeight;
    private Vector3 originalCCCenter;
    private float targetCameraY;
    private float originalCameraY;
    public float crouchSpeedMultiplier = 0.5f;
    public float cameraTransitionSpeed = 10f;
    [HideInInspector]
    public bool isCrouched = false;

    // State variable controlled by SeatInteraction.cs
    [HideInInspector]
    public bool isSeated = false;

    // *** NEW AUDIO VARIABLES ***
    [Header("Audio Settings")]
    [SerializeField] private AudioClip stepSFX; // Single footstep sound file
    private AudioSource audioSource;
    public float baseStepSpeed = 0.6f;     // Time in seconds between steps when walking
    public float runStepSpeedMultiplier = 0.4f; // Multiplier to make steps faster when running
    public float crouchStepSpeedMultiplier = 1.5f; // Multiplier to make steps slower when crouched
    private float stepTimer;

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
        Cursor.visible = false; // Hide cursor at start
        characterController = GetComponentInParent<CharacterController>();

        // *** NEW: Get AudioSource component ***
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            // Add AudioSource if missing and configure it
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
        }


        if (characterController != null)
        {
            // Store the initial settings exactly as they are configured in the Inspector
            originalCameraY = transform.localPosition.y;
            originalCCHeight = characterController.height;
            originalCCCenter = characterController.center;
            targetCameraY = originalCameraY;
        }
    }

    void Update()
    {
        if (characterController == null) return;

        // CRITICAL FIX: Block all movement and camera look if the game is paused
        if (PauseMenu.GameIsPaused)
        {
            return;
        }

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
                audioSource.PlayOneShot(stepSFX); // Play jump sound (optional)
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            }

            velocity.y += gravity * Time.deltaTime;

            // --- MOVEMENT ---
            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");

            float currentSpeed = moveSpeed;
            bool isMoving = (x != 0 || z != 0);
            bool isRunning = false;

            // Apply crouch speed multiplier
            if (isCrouched)
            {
                currentSpeed *= crouchSpeedMultiplier;
            }
            // Apply run speed if standing and running
            else if (Input.GetKey(KeyCode.LeftShift) && isMoving)
            {
                currentSpeed = runSpeed;
                isRunning = true;
            }

            Vector3 move = playerBody.right * x + playerBody.forward * z;
            characterController.Move((move * currentSpeed * Time.deltaTime) + (velocity * Time.deltaTime));

            // *** NEW: FOOTSTEP LOGIC ***
            if (isControllerGrounded && isMoving)
            {
                HandleFootsteps(isRunning);
            }
            else
            {
                // Reset timer if not moving or airborne
                stepTimer = 0f;
            }
        }

        // 2. --- CAMERA LOOK ---
        // ... (Camera Look logic remains the same) ...
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        float finalMouseX = mouseX * mouseSensitivity;
        float finalMouseY = mouseY * mouseSensitivity;

        // Apply Vertical Look
        xRotation -= finalMouseY;

        // Clamping vertical look
        float maxVerticalAngle = isSeated ? 45f : 90f;
        xRotation = Mathf.Clamp(xRotation, -maxVerticalAngle, maxVerticalAngle);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // Apply Horizontal Look
        if (isSeated)
        {
            playerBody.Rotate(Vector3.up * finalMouseX);
        }
        else
        {
            playerBody.Rotate(Vector3.up * finalMouseX);
        }
    }

    // *** NEW: FOOTSTEP HANDLER FUNCTION ***
    private void HandleFootsteps(bool isRunning)
    {
        // 1. Determine the time required for one step
        float stepDelay = baseStepSpeed;

        if (isRunning)
        {
            // Running makes steps much faster
            stepDelay *= runStepSpeedMultiplier;
        }
        else if (isCrouched)
        {
            // Crouching makes steps slower
            stepDelay *= crouchStepSpeedMultiplier;
        }

        // 2. Update the timer
        stepTimer += Time.deltaTime;

        // 3. Play sound when timer runs out
        if (stepTimer >= stepDelay)
        {
            if (audioSource != null && stepSFX != null)
            {
                // Randomize Pitch for natural variation
                audioSource.pitch = Random.Range(0.9f, 1.1f);

                // Play the single step clip
                audioSource.PlayOneShot(stepSFX);
            }
            // Reset timer
            stepTimer = 0f;
        }
    }

    // Function to handle the CharacterController height change and camera repositioning
    private void ApplyCrouchState()
    {
        // ... (ApplyCrouchState logic remains the same) ...
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