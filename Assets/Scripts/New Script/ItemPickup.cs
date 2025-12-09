using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    // Holding variables remain the same
    [Header("Holding Offset")]
    public Vector3 holdPosition = new Vector3(0.344f, -0.244f, 0.442f); 
    public Vector3 holdRotation = new Vector3(0f, -90f, 0f);           
    
    [Header("Weapon Settings")]
    public GameObject projectilePrefab;
    public float shootForce = 1500f;

    [Header("Audio File")]
    [SerializeField] private AudioClip shootSFX; // Suara yang akan dimainkan
    private AudioSource audioSource;

    // Other references
    private GameObject playerHand;
    private TV_Controller tvController; 
    private bool isHeld = false;
    private Transform mainCameraTransform; // Cached Camera reference
    
    // Scaling Variables
    public float scaleRaycastDistance = 10f;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        playerHand = GameObject.Find("hand"); 
        tvController = FindObjectOfType<TV_Controller>();

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
                // --- PISTOL SHOOTING LOGIC (Left Click) ---
                if (Input.GetMouseButtonDown(0))
                {
                    audioSource.clip = shootSFX;
                    audioSource.Play();
                    ShootProjectile();
                }
            }
            // Check if this item is the remote (based on its name)
            else if (gameObject.name.ToLower().Contains("remote"))
            {
                // --- REMOTE TV TOGGLE LOGIC (Left Click) ---
                if (Input.GetMouseButtonDown(0))
                {
                    if (tvController != null)
                    {
                        tvController.TogglePower();
                    }
                }
                
                // --- SCALING LOGIC (Right Click) ---
                CheckScalingInput();
            }
        }
    }

    // Function to handle the scaling raycast when holding the remote
    private void CheckScalingInput()
    {
        // Check for Right Click down (Input.GetMouseButtonDown(1))
        if (Input.GetMouseButtonDown(1)) 
        {
            // Raycast from the center of the camera
            Ray ray = mainCameraTransform.GetComponent<Camera>().ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            RaycastHit hit;
            
            if (Physics.Raycast(ray, out hit, scaleRaycastDistance))
            {
                // Check if the hit object has the Scalable component
                ScalableObject scalableTarget = hit.collider.GetComponent<ScalableObject>();
                if (scalableTarget != null)
                {
                    scalableTarget.ToggleScale();
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

        // 1. Instantiate the projectile
        GameObject projectile = Instantiate(
            projectilePrefab, 
            mainCameraTransform.position + mainCameraTransform.forward * 0.5f,
            mainCameraTransform.rotation
        );

        // 2. Get the Rigidbody and shoot it forward
        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.AddForce(mainCameraTransform.forward * shootForce);
        }
        
        // Increment the global counter
        if (OilCounterManager.Instance != null)
        {
            OilCounterManager.Instance.IncrementOilCount();
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
        isHeld = inHand;
        
        if (GetComponent<Rigidbody>() != null) GetComponent<Rigidbody>().isKinematic = inHand;
        if (GetComponent<Collider>() != null) GetComponent<Collider>().enabled = !inHand;
        
        if (playerHand != null)
        {
            playerHand.SetActive(!inHand); 
        }

        gameObject.SetActive(true);
    }
}