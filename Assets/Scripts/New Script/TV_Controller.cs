using UnityEngine;
using UnityEngine.Video; // Essential for VideoPlayer

public class TV_Controller : MonoBehaviour
{
    [Header("TV Setup")]
    // Drag your 'Screen' child object here (from your hierarchy: TV/Screen)
    public GameObject screenMesh; 
    
    // Drag the simple URP Unlit Material here (The Video_Ready_Mat)
    public Material screenOnMaterial; 
    
    // Drag the solid dark/black material here
    public Material screenOffMaterial; 

    private VideoPlayer videoPlayer;
    private MeshRenderer meshRenderer;
    private bool isTVOn = false;

    void Start()
    {
        // Get the required components from the assigned screenMesh object
        videoPlayer = screenMesh.GetComponent<VideoPlayer>();
        meshRenderer = screenMesh.GetComponent<MeshRenderer>();

        if (videoPlayer == null)
        {
            Debug.LogError("VideoPlayer component not found on screenMesh!");
            return;
        }

        // Start TV in the off state
        SetTVPower(false);
    }

    public void TogglePower()
    {
        isTVOn = !isTVOn;
        SetTVPower(isTVOn);
    }

    private void SetTVPower(bool on)
    {
        if (meshRenderer == null || videoPlayer == null) return;

        if (on)
        {
            // 1. Swap the material to the Video-Ready material (MANDATORY FIX)
            meshRenderer.material = screenOnMaterial;
            
            // 2. Start the video playback
            videoPlayer.Play();
        }
        else
        {
            // 1. Stop the video
            videoPlayer.Stop();
            
            // 2. Swap back to the dark 'off' material
            meshRenderer.material = screenOffMaterial;
        }
    }
}