using UnityEngine;

public class FPC_Controller : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float runSpeed = 10f; // NEW: The speed multiplier when running
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
    }

    void Update()
    {
        if (characterController == null) return; 

        // 1. --- GROUND CHECK and GRAVITY ---
        bool isControllerGrounded = characterController.isGrounded;

        if (isControllerGrounded && velocity.y < 0)
        {
            velocity.y = -0.5f; 
        }
        
        // --- JUMP (Spacebar OR PS4 Triangle button) ---
        if ((Input.GetButtonDown("Jump") || Input.GetKeyDown(KeyCode.JoystickButton3)) && isControllerGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;
        
        
        // 2. --- MOVEMENT (WASD OR Left Analog Stick) ---
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        // Determine the current speed based on input
        float currentSpeed = moveSpeed;
        
        // Check for the Run input (Left Shift key) and if we are pressing any movement key (x or z)
        // If the player is moving AND holding down the Left Shift key, use runSpeed.
        if (Input.GetKey(KeyCode.LeftShift) && (x != 0 || z != 0))
        {
            currentSpeed = runSpeed;
        }

        // Calculate horizontal movement vector
        Vector3 move = playerBody.right * x + playerBody.forward * z;

        // Apply movement using the determined currentSpeed
        characterController.Move((move * currentSpeed * Time.deltaTime) + (velocity * Time.deltaTime));


        // 3. --- CAMERA LOOK (Mouse OR Right Analog Stick) ---
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");
        
        float finalMouseX = mouseX * mouseSensitivity;
        float finalMouseY = mouseY * mouseSensitivity;

        xRotation -= finalMouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * finalMouseX);
    }
}