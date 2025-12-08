using UnityEngine;

public class SeatInteraction : MonoBehaviour
{
    // The empty GameObject that defines where the player sits (Drag your SitPoint here)
    [Header("Seat Point")]
    public Transform sitPoint; 
    
    // Tracks the current player object that is sitting
    [HideInInspector]
    public GameObject currentSeatedPlayer = null;

    // Public method to be called by Raycast_Interaction
    public void ToggleSeat(GameObject player)
    {
        if (currentSeatedPlayer == null)
        {
            // --- ACTION: SIT DOWN ---
            
            // 1. Store the player object
            currentSeatedPlayer = player;
            
            // 2. Disable the Character Controller for rigid movement
            CharacterController cc = player.GetComponent<CharacterController>();
            if (cc != null) cc.enabled = false;
            
            // 3. Move player to the sit point position
            player.transform.position = sitPoint.position;
            
            // 4. Align player rotation with the seat point (optional, but looks better)
            player.transform.rotation = sitPoint.rotation; 
            
            // 5. Tell the FPC_Controller that the player is seated
            FPC_Controller fpc = player.GetComponentInChildren<FPC_Controller>();
            if (fpc != null) fpc.isSeated = true;
            
            Debug.Log(player.name + " is now seated.");
        }
        else
        {
            // --- ACTION: STAND UP ---
            
            // 1. Tell the FPC_Controller the player is standing
            FPC_Controller fpc = currentSeatedPlayer.GetComponentInChildren<FPC_Controller>();
            if (fpc != null) fpc.isSeated = false;

            // 2. Re-enable the Character Controller
            CharacterController cc = currentSeatedPlayer.GetComponent<CharacterController>();
            if (cc != null) cc.enabled = true;
            
            // 3. Clear the reference
            currentSeatedPlayer = null;
            
            Debug.Log("Player stood up.");
        }
    }
    
    // Helper to check if the seat is available
    public bool IsAvailable()
    {
        return currentSeatedPlayer == null;
    }
}