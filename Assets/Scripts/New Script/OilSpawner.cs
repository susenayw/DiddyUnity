using UnityEngine;

public class OilSpawner : MonoBehaviour
{
    [Header("Spawner Setup")]
    public GameObject oilPrefab;
    public Transform spawnPoint;

    // Public method called by the Raycast_Interaction script
    public void SpawnOil()
    {
        if (oilPrefab == null || spawnPoint == null)
        {
            Debug.LogError("Oil Prefab or Spawn Point is not assigned in the OilSpawner script!");
            return;
        }

        // Instantiate the oil object
        GameObject newOil = Instantiate(oilPrefab, spawnPoint.position, spawnPoint.rotation);
        
        // Increment the global counter
        if (OilCounterManager.Instance != null)
        {
            OilCounterManager.Instance.IncrementOilCount();
        }
    }
}