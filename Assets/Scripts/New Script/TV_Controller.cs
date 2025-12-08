using UnityEngine;

public class TV_Controller : MonoBehaviour
{
    [Header("TV Setup")]
    // Drag the screen mesh object here to toggle its material/visibility
    public GameObject screenMesh; 
    public Material screenOnMaterial;
    public Material screenOffMaterial;

    private bool isTVOn = false;

    void Start()
    {
        // Ensure the TV starts in the off state
        SetTVPower(false);
    }

    // Public function called by the remote
    public void TogglePower()
    {
        isTVOn = !isTVOn;
        SetTVPower(isTVOn);
        
        Debug.Log("TV Power Toggled: " + isTVOn);
    }

    private void SetTVPower(bool on)
    {
        if (screenMesh == null)
        {
            Debug.LogError("Screen Mesh is not assigned in TV_Controller!");
            return;
        }

        // Change the screen material based on power state
        MeshRenderer renderer = screenMesh.GetComponent<MeshRenderer>();
        if (renderer != null)
        {
            renderer.material = on ? screenOnMaterial : screenOffMaterial;
        }
        
        // Optional: Play a sound or particles here
    }
}