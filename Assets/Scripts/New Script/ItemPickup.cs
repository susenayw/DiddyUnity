using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    // Holding variables remain the same
    [Header("Holding Offset")]
    public Vector3 holdPosition = new Vector3(0.344f, -0.244f, 0.442f); 
    public Vector3 holdRotation = new Vector3(0f, -90f, 0f);           
    
    // NEW: Reference to the Oil Prefab for shooting (Drag your Oil Prefab here!)
    [Header("Weapon Settings")]
    public GameObject projectilePrefab;
    public float shootForce = 1500f; // Force to push the oil forward

    // Other references
    private GameObject playerHand;
    private TV_Controller tvController; 
    private bool isHeld = false;
    
    // Cached Camera reference for shooting direction
    private Transform mainCameraTransform; 

    void Start()
    {
        playerHand = GameObject.Find("hand"); 
        tvController = FindObjectOfType<TV_Controller>();

        // Cache the camera transform for shooting direction
        if (Camera.main != null)
        {
            mainCameraTransform = Camera.main.transform;
        }
    }

    void Update()
    {
        if (isHeld)
        {
            // Check if this item is the pistol (based on its name)
            if (gameObject.name.ToLower().Contains("pistol"))
            {
                // --- PISTOL SHOOTING LOGIC ---
                if (Input.GetMouseButtonDown(0)) // Left Click
                {
                    ShootProjectile();
                }
            }
            else if (gameObject.name.ToLower().Contains("remote"))
            {
                // --- REMOTE TV LOGIC (Original) ---
                if (Input.GetMouseButtonDown(0))
                {
                    if (tvController != null)
                    {
                        tvController.TogglePower();
                    }
                }
            }
        }
    }

    private void ShootProjectile()
    {
        if (projectilePrefab == null || mainCameraTransform == null)
        {
            Debug.LogError("Projectile Prefab or Main Camera Transform is missing for shooting!");
            return;
        }

        // 1. Instantiate the projectile slightly in front of the pistol/camera
        GameObject projectile = Instantiate(
            projectilePrefab, 
            mainCameraTransform.position + mainCameraTransform.forward * 0.5f, // Spawn slightly forward
            mainCameraTransform.rotation // Match camera rotation
        );

        // 2. Get the Rigidbody and shoot it forward
        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.AddForce(mainCameraTransform.forward * shootForce);
        }
        
        Debug.Log("Fired: " + projectilePrefab.name);
    }


    // --- PickUp and SetInHand methods remain the same ---
    public void PickUp(Transform handParent)
    {
        transform.SetParent(handParent);
        SetInHand(true); 
        transform.localPosition = holdPosition;
        transform.localRotation = Quaternion.Euler(holdRotation.x, holdRotation.y, holdRotation.z);
    }
    
    public void SetInHand(bool inHand)
    {
        isHeld = inHand; // Update the held state
        
        // Physics and Collider management remains the same
        if (GetComponent<Rigidbody>() != null) GetComponent<Rigidbody>().isKinematic = inHand;
        if (GetComponent<Collider>() != null) GetComponent<Collider>().enabled = !inHand;
        
        // Hide/show the hand
        if (playerHand != null)
        {
            playerHand.SetActive(!inHand); 
        }

        gameObject.SetActive(true);
    }
}