using UnityEngine;

// Memastikan GameObject ini memiliki komponen AudioSource
[RequireComponent(typeof(AudioSource))]
public class OilSpawner : MonoBehaviour
{
    [Header("Spawner Setup")]
    public GameObject oilPrefab;
    public Transform spawnPoint;

    [Header("Audio Settings")]
    [SerializeField] private AudioClip spawnSFX; // Suara yang akan dimainkan
    private AudioSource audioSource;

    void Start()
    {
        // Mengambil komponen AudioSource saat game dimulai
        audioSource = GetComponent<AudioSource>();
    }

    // Public method called by the Raycast_Interaction script
    public void SpawnOil()
    {
        // --- 1. Mainkan Suara ---
        if (audioSource != null && spawnSFX != null)
        {
            // Memainkan suara spawn
            audioSource.PlayOneShot(spawnSFX);
        }

        // --- 2. Cek Setup ---
        if (oilPrefab == null || spawnPoint == null)
        {
            Debug.LogError("Oil Prefab or Spawn Point is not assigned in the OilSpawner script!");
            return;
        }

        // --- 3. Instantiate Object ---
        // Instantiate the oil object
        GameObject newOil = Instantiate(oilPrefab, spawnPoint.position, spawnPoint.rotation);

        // --- 4. Increment Counter ---
        // Increment the global counter
        if (OilCounterManager.Instance != null)
        {
            OilCounterManager.Instance.IncrementOilCount();
        }
    }
}