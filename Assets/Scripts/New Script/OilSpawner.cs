using UnityEngine;

public class OilSpawner : MonoBehaviour
{
    [Header("Spawner Setup")]
    // Drag your Oil Prefab into this slot in the Inspector
    public GameObject oilPrefab; 
    
    // Drag an empty GameObject (or the button's position) into this slot.
    // This defines where the oil will appear.
    public Transform spawnPoint; 

    // This is the function the Raycast_Interaction script will call
    public void SpawnOil()
    {
        if (oilPrefab == null || spawnPoint == null)
        {
            Debug.LogError("Oil Prefab or Spawn Point is not assigned in the OilSpawner script!");
            return;
        }

        // Instantiate the oil object at the specified spawn point position and rotation
        GameObject newOil = Instantiate(oilPrefab, spawnPoint.position, spawnPoint.rotation);
        
        Debug.Log("Oil spawned successfully!");
    }
}