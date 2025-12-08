using UnityEngine;

public class LampController : MonoBehaviour
{
    // The Light component that will actually illuminate the scene
    public Light sceneLight;

    // The name of the emissive color property in your shader (Standard/URP/HDRP)
    private const string EmissionColorName = "_EmissionColor"; 
    
    // The Renderer component that holds the material we want to change
    private Renderer lampRenderer; 
    
    // The actual material instance we will modify
    private Material lampMaterial;

    [Header("Emission Settings")]
    public Color baseEmissionColor = Color.white;
    public float emissionIntensityOn = 5.0f;
    public float emissionIntensityOff = 0.0f;

    private bool isLampOn = false;

    void Start()
    {
        // 1. Get the Renderer component attached to this GameObject
        lampRenderer = GetComponent<Renderer>();
        if (lampRenderer == null)
        {
            Debug.LogError("LampController requires a Renderer component!");
            enabled = false;
            return;
        }

        // 2. Get the material instance. We use .material to get a *copy* //    so we don't affect other objects using the same shared material.
        lampMaterial = lampRenderer.material;
        
        // Ensure the light is set up correctly at the start
        TurnOff(); 
    }

    // Call this method to toggle the lamp
    public void ToggleLamp()
    {
        if (isLampOn)
        {
            TurnOff();
        }
        else
        {
            TurnOn();
        }
    }

    public void TurnOn()
    {
        // 1. Visual Effect (Material Emission)
        // Set the emissive color by multiplying the base color by the intensity
        Color finalColor = baseEmissionColor * emissionIntensityOn;
        lampMaterial.SetColor(EmissionColorName, finalColor);
        
        // 2. Scene Illumination (Light Component)
        if (sceneLight != null)
        {
            sceneLight.enabled = true;
        }

        isLampOn = true;
    }

    public void TurnOff()
    {
        // 1. Visual Effect (Material Emission)
        // Set the emissive color to zero (off)
        Color finalColor = baseEmissionColor * emissionIntensityOff;
        lampMaterial.SetColor(EmissionColorName, finalColor);
        
        // 2. Scene Illumination (Light Component)
        if (sceneLight != null)
        {
            sceneLight.enabled = false;
        }

        isLampOn = false;
    }
}