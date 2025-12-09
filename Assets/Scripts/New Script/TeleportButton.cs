using UnityEngine;

public class TeleportButton : MonoBehaviour
{
    [Header("Teleport Target")]
    public Transform targetSpawnLocation;

    // Public method called by the Raycast_Interaction script
    public void TeleportPlayer(GameObject playerBody)
    {
        if (targetSpawnLocation == null)
        {
            Debug.LogError("Teleport target location is not set on the button: " + gameObject.name);
            return;
        }

        if (playerBody == null)
        {
            Debug.LogError("Player body object is null.");
            return;
        }

        // --- FIX: Get and temporarily disable the Character Controller ---
        CharacterController cc = playerBody.GetComponent<CharacterController>();
        if (cc != null)
        {
            // 1. Disable the controller
            cc.enabled = false;
        }

        // 2. Set the root Player object's position directly
        // Position offset: 1 unit up is used to prevent the player from clipping into the floor initially.
        playerBody.transform.position = targetSpawnLocation.position + Vector3.up * 1f;

        // 3. Re-enable the controller
        if (cc != null)
        {
            cc.enabled = true;
        }
        
        Debug.Log("Player successfully teleported to: " + targetSpawnLocation.name);
    }
}